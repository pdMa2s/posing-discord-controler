﻿using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GestureModality
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged, IDisposable
    {
        private KinectSensor kinectSensor;
        private Body[] bodies;
        private int activeBodyIndex;
        private BodyFrameReader bodyFrameReader;
        private GestureDetector gestureDetector;
        internal static MainWindow main;
        private ComModule coms;
        private List<Guild> guildList;
        // INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            this.InitializeComponent();
            main = this;

            KinectRegion.SetKinectRegion(this, kinectRegion);

            App app = ((App)Application.Current);
            app.KinectRegion = kinectRegion;

            this.kinectSensor = KinectSensor.GetDefault();

            // Use the default sensor
            this.kinectRegion.KinectSensor = this.kinectSensor;

            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;
            guildList = new List<Guild>();
            coms = new ComModule(this);

            this.gestureDetector = new GestureDetector(kinectSensor,coms);
            this.activeBodyIndex = -1;

            Guild guild = new Guild("IMServer");
            //Guild guild2 = new Guild("Topicos de Apicultura");
            //AddGuild(guild);
            //AddGuild(guild2);
            /*if (!kinectSensor.IsAvailable)
            {
                Console.WriteLine("Kinect Sensor is not available!");
                Environment.Exit(1);
            }*/
        }

        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            
            bool bodyInFrame = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (bodies == null)
                    {
                        // creates an array of 6 bodies, which is the max number of bodies that Kinect can track simultaneously
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }
                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    VerifyActiveBody();
                    bodyInFrame = this.activeBodyIndex != -1;
                }
            }

            if (bodyInFrame)
            {
                Body body = this.bodies[this.activeBodyIndex];

                // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                if (body.TrackingId != this.gestureDetector.TrackingId)
                    this.gestureDetector.TrackingId = body.TrackingId;

                // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                this.gestureDetector.IsPaused = (body.TrackingId == 0);
            }

        }

        private void VerifyActiveBody()
        {
            this.activeBodyIndex = -1;

            int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;

            float minZPoint = float.MaxValue; // Default to impossible value
            for (int i = 0; i < maxBodies; i++)
            {
                Body body = this.bodies[i];
                if (body.IsTracked)
                {
                    float zMeters = body.Joints[JointType.SpineBase].Position.Z;
                    if (zMeters < minZPoint)
                    {
                        minZPoint = zMeters;
                        this.activeBodyIndex = i;
                    }
                }
            }
        }

        internal string ChangeDetectedGesture
        {
            get { return this.gestureDetected.Text.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { this.gestureDetected.Text = value; })); }
        }

        internal string ChangeConfidence
        {
            get { return this.confidence.Text.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { this.confidence.Text = value; })); }
        }

        private Button CreateButton(string name, double marginWidth, double marginHeight)
        {
            Button newButton = new Button();
            newButton.Content = name;
            newButton.Name = name + "BTN";
            newButton.Width = 15 * name.Length;
            newButton.Height = 50;
            newButton.HorizontalAlignment = HorizontalAlignment.Left;
            newButton.VerticalAlignment = VerticalAlignment.Top;
            newButton.Style = FindResource("buttonStyle") as Style;
            newButton.Click += channelsButtonClicked;
            Thickness margin = newButton.Margin;
            margin.Left = marginWidth;
            margin.Top = marginHeight;
            newButton.Margin = margin;
            return newButton;
        }

        public void AddChannelsToGUI(string[] channelsName)
        {
            double marginWidth = 10;
            double marginHeight = 10;
            for (int i = 0; i < channelsName.Length; i++)
            {
                Button button = CreateButton(channelsName[i], marginWidth, marginHeight);
                gridChannels.Children.Add(button);
                marginWidth += button.Width + 25;
            }
        }

        private void channelsButtonClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string channelNameSelectedPrev = this.gestureDetector.ChannelName;
            string channelNameSelectedNow = button.Content as string;
            Console.WriteLine("Channel Selected: " + this.gestureDetector.ChannelName);
            if (channelNameSelectedNow.Equals(channelNameSelectedPrev))
            {
                button.Background = Brushes.DarkTurquoise;
                this.gestureDetector.ChannelName = null;
                Console.WriteLine("Channel Selected: " + this.gestureDetector.ChannelName);
                return;
            }
            this.gestureDetector.ChannelName = button.Content as string;
            if (channelNameSelectedPrev != null)
            {
                for (int i = 0; i < gridChannels.Children.Count; i++)
                {
                    Button children = gridChannels.Children[i] as Button;
                    string content = children.Content as string;
                    if (content.Equals(channelNameSelectedPrev))
                    {
                        children.Background = Brushes.DarkTurquoise;
                        break;
                    }
                }
            }
            button.Background = Brushes.LimeGreen;
            Console.WriteLine("Channel Selected: " + this.gestureDetector.ChannelName);
        }

        public void AddUsersToGUI(string[] usersName)
        {
            double marginWidth = 10;
            double marginHeight = 10;
            for (int i = 0; i < usersName.Length; i++)
            {
                Button newButton = CreateButton(usersName[i], marginWidth, marginHeight);
                gridUsers.Children.Add(newButton);
                marginWidth += newButton.Width + 25;
            }
        }

        private void usersButtonClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string userNameSelectedPrev = this.gestureDetector.UserName;
            string userNameSelectedNow = button.Content as string;
            Console.WriteLine("User Selected: " + this.gestureDetector.UserName);
            if (userNameSelectedNow.Equals(userNameSelectedPrev))
            {
                button.Background = Brushes.DarkTurquoise;
                this.gestureDetector.UserName = null;
                Console.WriteLine("User Selected: " + this.gestureDetector.UserName);
                return;
            }
            this.gestureDetector.UserName = button.Content as string;
            if (userNameSelectedPrev != null)
            {
                for (int i = 0; i < gridUsers.Children.Count; i++)
                {
                    Button children = gridUsers.Children[i] as Button;
                    string content = children.Content as string;
                    if (content.Equals(userNameSelectedPrev))
                    {
                        children.Background = Brushes.DarkTurquoise;
                        break;
                    }
                }
            }
            button.Background = Brushes.LimeGreen;
            Console.WriteLine("User Selected: " + this.gestureDetector.UserName);
        }

        public void ChangeColorBTNUserSelected(string userName)
        {
            for (int i = 0; i < gridUsers.Children.Count; i++)
            {
                Button children = gridUsers.Children[i] as Button;
                string content = children.Content as string;
                if (content.Equals(userName))
                {
                    children.Background = Brushes.DarkTurquoise;
                    break;
                }
            }
        }

        public void ChangeColorBTNChannelSelected(string channelName)
        {
            for (int i = 0; i < gridChannels.Children.Count; i++)
            {
                Button children = gridChannels.Children[i] as Button;
                string content = children.Content as string;
                if (content.Equals(channelName))
                {
                    children.Background = Brushes.DarkTurquoise;
                    break;
                }
            }
        }

        public void AddGuild(Guild guild)
        {
            this.guildList.Add(guild);
            if (this.gridGuilds.Children.Count == 0)
            {
                Button button = CreateButton(guild.Name, 10, 10);
                this.gridGuilds.Children.Add(button);
            }
            else
            {
                int numChildren = this.gridGuilds.Children.Count;
                Button button = this.gridGuilds.Children[numChildren-1] as Button;
                Button newButton = CreateButton(guild.Name, button.Margin.Left+button.Width+25, button.Height);
                this.gridGuilds.Children.Add(newButton);
            }
        }

        private void helpButtonClicked(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow(kinectSensor);
            helpWindow.Show();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Disposes the GestureDetector object
        // True if Dispose was called directly, false if the GC handles the disposing
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.gestureDetector != null)
                {
                    this.gestureDetector.Dispose();
                    this.gestureDetector = null;
                }
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.bodyFrameReader != null)
            {
                this.bodyFrameReader.FrameArrived -= this.Reader_BodyFrameArrived;
                this.bodyFrameReader.Dispose();
                this.bodyFrameReader = null;
            }

            if (this.gestureDetector != null)
            {
                this.gestureDetector.Dispose();
                this.gestureDetector = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }

        }
    }
}

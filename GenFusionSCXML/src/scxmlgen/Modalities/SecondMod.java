package scxmlgen.Modalities;

import scxmlgen.interfaces.IModality;

/**
 *
 * @author nunof
 */
public enum SecondMod implements IModality{

    MUTE_MATOS("[action][MUTE_USER][userName][Matos]", 1500),
    MUTE_AMBROSIO("[action][MUTE_USER][userName][Ambrosio]", 1500),
    MUTE_GUSTAVO("[action][MUTE_USER][userName][Gustavo]", 1500),
    SELF_MUTE("[action][SELF_MUTE]", 1500),
    MUTE_MATOS_IMSERVER("[action][MUTE_USER][userName][Matos][guildName][IMServer]", 1500),
    MUTE_MATOS_TOPICOS("[action][MUTE_USER][userName][Matos][guildName][Topicos de Apicultura]", 1500),
    MUTE_AMBROSIO_IMSERVER("[action][MUTE_USER][userName][Ambrosio][guildName][IMServer]", 1500),
    MUTE_AMBROSIO_TOPICOS("[action][MUTE_USER][userName][Ambrosio][guildName][Topicos de Apicultura]", 1500),
    MUTE_GUSTAVO_IMSERVER("[action][MUTE_USER][userName][Gustavo][guildName][IMServer]", 1500),
    MUTE_GUSTAVO_TOPICOS("[action][MUTE_USER][userName][Gustavo][guildName][Topicos de Apicultura]", 1500),
    SELF_MUTE_IMSERVER("[action][SELF_MUTE][guildName][IMServer]", 1500),
    SELF_MUTE_TOPICOS("[action][SELF_MUTE][guildName][Topicos de Apicultura]", 1500),
    
    DEAF_MATOS("[action][DEAF_USER][userName][Matos]", 1500),
    DEAF_AMBROSIO("[action][DEAF_USER][userName][Ambrosio]", 1500),
    DEAF_GUSTAVO("[action][DEAF_USER][userName][Gustavo]", 1500),
    SELF_DEAF("[action][SELF_DEAF]", 1500),
    DEAF_MATOS_IMSERVER("[action][DEAF_USER][userName][Matos][guildName][IMServer]", 1500),
    DEAF_MATOS_TOPICOS("[action][DEAF_USER][userName][Matos][guildName][Topicos de Apicultura]", 1500),
    DEAF_AMBROSIO_IMSERVER("[action][DEAF_USER][userName][Ambrosio][guildName][IMServer]", 1500),
    DEAF_AMBROSIO_TOPICOS("[action][DEAF_USER][userName][Ambrosio][guildName][Topicos de Apicultura]", 1500),
    DEAF_GUSTAVO_IMSERVER("[action][DEAF_USER][userName][Gustavo][guildName][IMServer]", 1500),
    DEAF_GUSTAVO_TOPICOS("[action][DEAF_USER][userName][Gustavo][guildName][Topicos de Apicultura]", 1500),
    SELF_DEAF_IMSERVER("[action][SELF_DEAF][guildName][IMServer]", 1500),
    SELF_DEAF_TOPICOS("[action][SELF_DEAF][guildName][Topicos de Apicultura]", 1500),
    
    KICK_USER("[action][REMOVE_USER]",1500),
    KICK_USER_IMSERVER("[action][REMOVE_USER][guildName][IMServer]",1500),
    KICK_USER_TOPICOS("[action][REMOVE_USER][guildName][Topicos de Apicultura]",1500),
    
    BAN_USER("[action][BAN_USER]",1500),
    BAN_USER_IMSERVER("[action][BAN_USER][guildName][IMServer]",1500),
    BAN_USER_TOPICOS("[action][BAN_USER][guildName][Topicos de Apicultura]",1500),
    
    DELETE_MESSAGE("[action][DELETE_LAST_MESSAGE]",1500),
    DELETE_MESSAGE_IMSERVER("[action][DELETE_LAST_MESSAGE][guildName][IMServer]",1500),
    DELETE_MESSAGE_TOPICOS("[action][DELETE_LAST_MESSAGE][guildName][Topicos de Apicultura]",1500),
    ;
    
    
    private String event;
    private int timeout;


    SecondMod(String m, int time) {
        event=m;
        timeout=time;
    }

    @Override
    public int getTimeOut() {
        return timeout;
    }

    @Override
    public String getEventName() {
        //return getModalityName()+"."+event;
        return event;
    }

    @Override
    public String getEvName() {
        return getModalityName().toLowerCase()+event.toLowerCase();
    }
    
}
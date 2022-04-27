using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Journal
{
    public enum SuspectColors
    {
        None,
        Red,
        Blue,
        Green,
        Yellow,
        Purple
    }
    
    public enum Relationships
    {
        None,
        Friend,
        Husband,
        Wife,
        Boyfriend,
        Girlfriend,
        Lover,
        Parent,
        Child,
        Pet,
        Boss,
        Subordinate
    }

    public enum Suspects
    {
        None,
        JHarlow,
        LDubois,
        JMalone,
        DButterbur,
        PHarlow
    }

    public enum Actions
    {
        None,
        Shot,
        Stabbed,
        Poisoned,
        Bludgeoned,
        Fell,
        Kicked,
        Punched,
        Choked,
        Bitten,
        Overdosed
    }

    public enum Locations
    {
        None,
        LadiesBathroom,
        MensBathroom,
        Alleyway,
        MainBar,
        Office
    }

    public enum Motives
    {
        None,
        Debt,
        Love,
        Hate,
        Envy,
        Loyalty,
        Drugs
    }
}

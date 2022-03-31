using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using UnityEngine;

[CreateAssetMenu(fileName = "Relationships", menuName = "Journal/RelationshipObject")]
public class Relationship : ScriptableObject
{
    [Serializable]
    public class RelationshipPair
    {
        public Suspects suspect1;
        public Suspects suspect2;
        public Relationships relationship1;
        public Relationships relationship2;

        public bool CheckCorrect(Suspects inSuspect1, Suspects inSuspect2, Relationships inRelationship1, Relationships inRelationship2)
        {
            if (suspect1 != inSuspect1) return false;
            if (suspect2 != inSuspect2) return false;
            if (relationship1 != inRelationship1) return false;
            if (relationship2 != inRelationship2) return false;

            return true;
        }
    }
    
    public List<RelationshipPair> relationshipsList;
    
    
    public bool CheckCorrect(Suspects inSuspect1, Suspects inSuspect2, Relationships inRelationship1, Relationships inRelationship2)
    {
        int index = GetIndex(inSuspect1, inSuspect2);
        if (index == -1) return false;
        
        return relationshipsList[index].CheckCorrect(inSuspect1, inSuspect2, inRelationship1, inRelationship2);
    }

    #region Helper Functions
    private int GetIndex(Suspects inSuspect1, Suspects inSuspect2)
    {
        for (int ii = 0; ii < relationshipsList.Count; ii++)
        {
            RelationshipPair pair = relationshipsList[ii];
            if (inSuspect1 == pair.suspect1 && inSuspect2 == pair.suspect2 || inSuspect1 == pair.suspect2 && inSuspect2 == pair.suspect1)
            {
                return ii;
            }
        }

        return -1;
    }
    #endregion
}

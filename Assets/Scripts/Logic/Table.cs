using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Logic
{
    public class Table : MonoBehaviour
    {
        private List<CreatureLogic> _creaturesOnTable = new List<CreatureLogic>();
        public List<CreatureLogic> CreaturesOnTable => _creaturesOnTable;

        public void PlaceCreatureAt(int index, CreatureLogic creature)
        {
            _creaturesOnTable.Insert(index, creature);
        }
    }
}
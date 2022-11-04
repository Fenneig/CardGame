using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardGame.Logic
{
    public class IDHolder : MonoBehaviour
    {
        private int _uniqueID;
        private static List<IDHolder> _allIDHolders = new List<IDHolder>();

        public int UniqueID
        {
            get => _uniqueID;
            set => _uniqueID = value;
        }

        private void Awake()
        {
            _allIDHolders.Add(this);
        }

        public static GameObject GetGameObjectWithID(int ID)
            => (from idHolder in _allIDHolders where idHolder._uniqueID == ID select idHolder.gameObject)
                .FirstOrDefault();

        public static void ClearIDHoldersList() =>
            _allIDHolders.Clear();
    }
}
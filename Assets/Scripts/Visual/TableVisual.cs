using System;
using System.Collections.Generic;
using CardGame.Commands;
using CardGame.Logic;
using CardGame.SOAssets;
using CardGame.Visual.CardVisual;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Visual;

namespace CardGame.Visual
{
    public class TableVisual : MonoBehaviour
    {
        [SerializeField] private AreaPosition _owner;
        [SerializeField] private SameDistanceChildren _slots;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private float _creatureMoveTime = 0.3f;

        private List<GameObject> _creaturesOnTable = new List<GameObject>();
        private bool _cursorOverThisTable = false;
        private bool CursorOverThisTable => _cursorOverThisTable;

        public static bool CursorOverSomeTable
        {
            get
            {
                TableVisual[] bothTables = FindObjectsOfType<TableVisual>();
                return bothTables[0].CursorOverThisTable || bothTables[1].CursorOverThisTable;
            }
        }

        private void Update()
        {
            RaycastHit[] hits;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            hits = Physics.RaycastAll(ray.origin, ray.direction, 100f);

            bool passedThroughTableCollider = false;
            foreach (var hit in hits)
                if (hit.collider == _collider) passedThroughTableCollider = true;
            
            _cursorOverThisTable = passedThroughTableCollider;
        }

        public void AddCreatureAtIndex(CardAsset cardAsset, int uniqueID, int index)
        {
            GameObject creature = Instantiate(GlobalSettings.Instance.CreaturePrefab,
                _slots.Children[index].transform.position, quaternion.identity);
            OneCreatureManager manager = creature.GetComponent<OneCreatureManager>();
            manager.CardAsset = cardAsset;
            manager.ReadCreatureFromAsset();

            foreach (var transform in creature.GetComponentsInChildren<Transform>())
                transform.tag = $"{_owner}Creature";

            creature.transform.SetParent(_slots.transform);

            _creaturesOnTable.Insert(index, creature);

            CardsAndCreaturesVisualState creatureState = creature.GetComponent<CardsAndCreaturesVisualState>();
            creatureState.Slot = index;
            creatureState.State = VisualStates.BottomTable;

            IDHolder idHolder = creature.AddComponent<IDHolder>();
            idHolder.UniqueID = uniqueID;

            ShiftSlotsGameObjectAccordingToNumberOfCreatures();
            PlaceCreaturesOnNewSlots();
            
            Command.CommandExecutionComplete();
        }

        public int TablePositionForNewCreature(float mouseX)
        {
            if (_creaturesOnTable.Count == 0 || mouseX > _slots.Children[0].transform.position.x) 
                return 0;
            
            if (mouseX < _slots.Children[_creaturesOnTable.Count - 1].transform.position.x) 
                return _creaturesOnTable.Count;
            
            for (int i = 0; i < _creaturesOnTable.Count - 1; i++)
            {
                if (mouseX < _slots.Children[i].transform.position.x &&
                    mouseX > _slots.Children[i + 1].transform.position.x) return i + 1;
            }
            Debug.Log("Suspicious behavior. Reached end of TablePositionForNewCreature method. Returning 0");
            return 0;
        }

        public void RemoveCreatureWithID(int IDToRemove)
        {
            GameObject creatureToRemove = IDHolder.GetGameObjectWithID(IDToRemove);
            _creaturesOnTable.Remove(creatureToRemove);
            Destroy(creatureToRemove);
            
            ShiftSlotsGameObjectAccordingToNumberOfCreatures();
            PlaceCreaturesOnNewSlots();
            Command.CommandExecutionComplete();
        }

        private void ShiftSlotsGameObjectAccordingToNumberOfCreatures()
        {
            float posX;
            if (_creaturesOnTable.Count > 0)
                posX = (_slots.Children[0].transform.localPosition.x - _slots.Children[_creaturesOnTable.Count - 1].transform.localPosition.x) / 2;
            else posX = 0f;

            _slots.gameObject.transform.DOLocalMoveX(posX, _creatureMoveTime);
        }

        private void PlaceCreaturesOnNewSlots()
        {
            foreach (var creature in _creaturesOnTable)
                creature.transform.DOLocalMoveX(
                    _slots.Children[_creaturesOnTable.IndexOf(creature)].transform.localPosition.x,
                    _creatureMoveTime);
        }
    }
}
using Entities;
using Players;
using UnityEngine;

public class PlayerCustomController : MonoBehaviour
{
    [SerializeField] private Player player;

    public void SetPlayerMove(bool value) => player.GetCompo<EntityMover>().CanManualMove = value;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistic : MonoBehaviour
{
    public float playerSpeed = 1.0f;
    public float jumpHeigth = 3.0f;
    public float gravity = 20.0f;
    public float stepDown = 0.2f;
    public float airControl = 0.5f;
    public float jumpDamp = 0.5f;
    public bool isJumping;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformState 
{
    Vector2 MovePlatform(Platform platform);


    Vector2 RefreshPlatform();
}

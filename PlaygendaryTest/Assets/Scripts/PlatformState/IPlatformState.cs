using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformState
{
    void MovePlatform(Platform platform, float oldDistance, float newDistance);
}

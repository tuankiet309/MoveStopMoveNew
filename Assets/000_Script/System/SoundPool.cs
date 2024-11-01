using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool : ObjectPoolAbtract<SoundSource>
{
    [SerializeField] private SoundSource m_Source;
    protected override SoundSource CreateObject()
    {
        SoundSource newAudioSource = Instantiate(m_Source);
        return newAudioSource;
    }
}

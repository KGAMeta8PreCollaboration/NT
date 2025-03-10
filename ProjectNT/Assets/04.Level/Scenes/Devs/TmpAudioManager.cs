using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
	HIT1,
	HIT2,
	HIT3,
	HIT4,
}

public class TmpAudioManager : MonoBehaviour
{
	public static TmpAudioManager instance;
	
	[Header("SFX")]
	// 여러 오디오 채널을 그룹으로 묶어 관리할 수 있는 객체입니다.
	// 이 그룹에 속한 모든 채널에 대해 볼륨 조절, 일시 정지, 재생 등의 작업을 일괄적으로 수행할 수 있습니다.
	private FMOD.ChannelGroup sfxChannelGroup;
	
	// 개별적인 오디오 클립을 나타냅니다.
	// 이 배열은 여러 개의 사운드 효과(SFX)를 저장하는 데 사용됩니다.
	private FMOD.Sound[] sfxs;
	
	// FMOD.Channel 객체의 배열로, 각각의 FMOD.Channel 객체는 현재 재생 중인 오디오 스트림을 나타냅니다.
	// 이 배열은 여러 개의 사운드 효과가 재생되는 채널을 관리하는 데 사용됩니다.
	private FMOD.Channel[] sfxChannels;

	
}

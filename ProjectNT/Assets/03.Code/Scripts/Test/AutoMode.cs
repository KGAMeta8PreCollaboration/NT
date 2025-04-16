using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMode : MonoBehaviour
{
    public double perfectThreshold;

    public Woofer[] woofers;
    public List<NoteRail> rails;
    public LinkedList<Note>[] noteList;

    private WooferNetworkSync _wooferNetworkSync;

    private void Start()
    {
        _wooferNetworkSync = FindObjectOfType<WooferNetworkSync>();

        // PlayerModule myModule = GameManager.Instance.MultiGameController.GetPlayerModuleByNick(PhotonNetwork.LocalPlayer.NickName);

        // woofers = myModule.woofers;
        // rails = myModule.NoteManager.noteRails;

        noteList = new LinkedList<Note>[rails.Count];

        for (int i = 0; i < rails.Count; i++)
        {
            noteList[i] = rails[i].GetNoteList();
            StartCoroutine(AutoModeRail(i, i < 4 ? false : true));
        }
    }

    public IEnumerator AutoModeRail(int index, bool topRail)
    {
        while (true)
        {
            if (noteList[index].Count > 0)
            {
                Note note = noteList[index].First.Value;
                double currentTime = AudioSettings.dspTime;
                double targetTime = note.GetTargetDspTime();
                double timeDiff = Math.Abs(currentTime - targetTime);

                bool isLongNote = note is LongNote;

                // 첫 정박 타이밍 처리
                if (timeDiff < perfectThreshold && !note.isHit)
                {
                    if (!topRail)
                    {
                        if (!GameManager.Instance.IsMulti) woofers[index].Hit();
                        else _wooferNetworkSync.SendHit(woofers[index], PhotonNetwork.LocalPlayer.NickName);
                    }
                    else
                    {
                        TopNote topNote = note as TopNote;
                        topNote?.AutoHit(new UnityEngine.InputSystem.InputAction.CallbackContext());

                        if (!GameManager.Instance.IsMulti) topNote?.AutoHit(new UnityEngine.InputSystem.InputAction.CallbackContext());
                        else _wooferNetworkSync.SendTopNoteHit(topNote, PhotonNetwork.LocalPlayer.NickName);
                    }

                    // Debug.Log($"첫 Hit 처리됨 - 시간차: {timeDiff:f2}");
                }
                // 롱노트면 홀드 처리
                else if (isLongNote && note.isHit)
                {
                    if (!GameManager.Instance.IsMulti) woofers[index].Hold();
                    else _wooferNetworkSync.SendHold(woofers[index], PhotonNetwork.LocalPlayer.NickName);
                    // Debug.Log($"Hold 처리 중 - 현재: {currentTime:f2}");
                }
            }

            yield return null;
        }
    }

}
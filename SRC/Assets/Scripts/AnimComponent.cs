using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimComponent : MonoBehaviour, ISerializationCallbackReceiver
{
	public StringAnimDataPair[] Datas;
	public string CurrentAnimKey;
	public SpriteRenderer GraphicTarget;

	private Dictionary<string, AnimData> _datas;
	private IEnumerator _routineToIterate;


	private void Reset()
	{
		CurrentAnimKey = "Idle";
		_datas = new Dictionary<string, AnimData>()
		{
			{ "Idle", new AnimData() }
		};
	}

	private void Start()
	{
		PlayAnim(CurrentAnimKey);
	}

	private void Update()
	{
		if (_routineToIterate != null)
			_routineToIterate.MoveNext();
	}

	public void PlayAnim(string KeyAnim)
	{
		if (string.IsNullOrEmpty(KeyAnim) || _datas == null || !_datas.ContainsKey(KeyAnim))
			return;
		else
		{
			CurrentAnimKey = KeyAnim;
			_routineToIterate = _datas[KeyAnim].Play(SetSprite);
		}
	}

	private void SetSprite(Sprite NewSprite)
	{
		GraphicTarget.sprite = NewSprite;
	}


	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		_datas = new Dictionary<string, AnimData>();
		if (Datas == null)
			return;
		for (int i = 0, iLength = Datas.Length; i < iLength; i++)
		{
			var key = i != 0 && Datas[i].Key == Datas[i - 1].Key ? Datas[i].Key + "0" : Datas[i].Key;
			_datas[key] = Datas[i].Value;
		}
		Datas = null;
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		if (_datas == null)
			return;
		List<StringAnimDataPair> listTmp = new List<StringAnimDataPair>();
		foreach(var keyvalue in _datas)
		{
			listTmp.Add(new StringAnimDataPair(keyvalue.Key, keyvalue.Value));
		}

		Datas = listTmp.ToArray();
		_datas = null;
	}
}

[System.Serializable]
public class AnimData
{
	public Sprite[] AnimFrames;
	public float Duration;

	public Sprite CurrentFrame { get; private set; }

	public IEnumerator Play(System.Action<Sprite> FctDisplaySprite)
	{
		int lengthFrames = AnimFrames.Length;
		float durationEachFrames = Duration / lengthFrames;
		float timeLeft = durationEachFrames;
		int indexFrames = 0;

		CurrentFrame = AnimFrames[indexFrames];

		FctDisplaySprite(CurrentFrame);

		while (true)
		{
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0f)
			{
				indexFrames = (indexFrames + 1 ) % lengthFrames;
				timeLeft = durationEachFrames;
				CurrentFrame = AnimFrames[indexFrames];
				FctDisplaySprite(CurrentFrame);
			}
			yield return null;
		}
	}
}

[System.Serializable]
public class StringAnimDataPair
{
	public string Key;
	public AnimData Value;

	public StringAnimDataPair(string Key, AnimData Value)
	{
		this.Key = Key;
		this.Value = Value;
	}
}

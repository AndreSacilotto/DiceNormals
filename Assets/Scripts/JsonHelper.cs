﻿using UnityEngine;

public static class JsonHelper
{
	public static T[] FromJsonArray<T>(string json)
	{
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
		return wrapper.Items;
	}

	public static string ToJsonArray<T>(T[] array)
	{
		Wrapper<T> wrapper = new Wrapper<T>
		{
			Items = array
		};
		return JsonUtility.ToJson(wrapper);
	}

	public static string ToJsonArray<T>(T[] array, bool prettyPrint)
	{
		Wrapper<T> wrapper = new Wrapper<T>
		{
			Items = array
		};
		return JsonUtility.ToJson(wrapper, prettyPrint);
	}

	[System.Serializable]
	private class Wrapper<T>
	{
		public T[] Items;
	}
}
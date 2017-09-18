using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TypeOfSpawner {BYTAG, COMPLEX}

[CreateAssetMenu(fileName = "EnemySpawner", menuName = "Spawner", order = 4)]
public class EnemySpawnerData : ScriptableObject {

	public TypeOfSpawner selectionType;
	public float[] chancesByTag;
	public float[,] chancesComplex;
}

[CustomEditor(typeof(EnemySpawnerData))]
public class EnemySpawnEditor : Editor {

	public override void OnInspectorGUI() {
		EnemySpawnerData spawnData = (EnemySpawnerData)target;

		spawnData.selectionType = (TypeOfSpawner)EditorGUILayout.EnumPopup(spawnData.selectionType);

		//By Tag
		if(spawnData.selectionType == TypeOfSpawner.BYTAG) {
			if (spawnData.chancesByTag == null) {
								
				spawnData.chancesByTag = new float[System.Enum.GetNames(typeof(EnemyTag)).Length];


			}

			EditorGUILayout.BeginVertical("Box");

			for (int i = 0; i < spawnData.chancesByTag.Length; i++) {

				EditorGUILayout.BeginHorizontal("Button");
				EditorGUILayout.LabelField(((EnemyTag) (1 << i)).ToString());
				spawnData.chancesByTag[i] = EditorGUILayout.Slider(spawnData.chancesByTag[i], 0.0f, 100.0f);
				EditorGUILayout.EndHorizontal();

			}

			EditorGUILayout.EndVertical();


		} 
		
		//By Complex
		if(spawnData.selectionType == TypeOfSpawner.COMPLEX) {

			int a;
			int b;

			a = System.Enum.GetNames(typeof(EnemyGroup)).Length;
			b = numberOfEnemiesOnTheBiggerGroup();

			if (spawnData.chancesComplex == null) {				
				
				spawnData.chancesComplex = new float[a,b];

				}

			Debug.Log("Pinbg");

			for (int i = 0; i < a; i++) {
				
				GUILayout.BeginVertical("Box");
				EditorGUILayout.LabelField(((EnemyGroup)(1<<i)).ToString());

				for (int j = 0; j < b; j++) {



				}

				GUILayout.EndVertical();


			}


		}

	}

	public int numberOfEnemiesOnTheBiggerGroup() {

		int a=0;
		int b=0;

		int[] enemiesPerGroup;
		Object[] allEnemies;

		a = System.Enum.GetNames(typeof(EnemyGroup)).Length;
		enemiesPerGroup = new int[a];

		allEnemies = Resources.LoadAll("EnemyTags", typeof(EnemyInfoData));

		for (int i = 0; i < allEnemies.Length; i++) {

			for (int j = 0; j < a; j++) {
				if ((((EnemyInfoData)allEnemies[i]).groups & ((EnemyGroup)(1 << j))) == ((EnemyGroup)(1 << j))) {
					enemiesPerGroup[j]++;
				}
			}
		}

		for (int i = 0; i < enemiesPerGroup.Length; i++) {
			if (b <= enemiesPerGroup[i]) {
				b = enemiesPerGroup[i];
			}
		}

		return b;

	}

}



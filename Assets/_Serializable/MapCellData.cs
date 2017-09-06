using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Tile {
	VAZIO,
	CHAO,
	PORTA,
	LAVA,
	ESPINHO,
	PAREDE_L, PAREDE_O, PAREDE_N, PAREDE_S, PAREDE_NL, PAREDE_NO, PAREDE_SL, PAREDE_SO
}
public enum Limite {PAREDE, PORTA, CONEXAO}

public struct EnemySpawnerData {
	EnemySpawner es;
	int x, y;
}

[CreateAssetMenu(fileName ="NewCell", menuName ="Map Cell", order =1)]
public class MapCellData : ScriptableObject {

	public int dimensao = 15;

	public Tile[,] blueprint;
	public Limite[] limites;
	public EnemySpawnerData[] esd;
}

[CustomEditor(typeof(MapCellData))]
public class MapCellDataCustomEditor : Editor {
	class Colors {
		public Color vazio;
		public Color chao;
		public Color parede;
		public Color porta;
		public Color lava;
		public Color espinho;

		public Colors () {
			vazio = new Color(.117f, .117f, .117f);
			chao = new Color(.349f, .349f, .349f);
			parede = new Color(.486f, .270f, .035f);
			porta = new Color(.490f, .176f, .658f);
			lava = new Color(.717f, .176f, 0f);
			espinho = new Color(0f, .267f, 0f);
		}
	}


	private GUIStyle guiStyleColor;
	private Colors colors;

	

	void initializeGuiStyleColor() {
		if (guiStyleColor == null) {
			guiStyleColor = new GUIStyle(EditorStyles.popup);
			guiStyleColor.normal.textColor = Color.white;
			guiStyleColor.hover.textColor = Color.white;
			guiStyleColor.focused.textColor = Color.white;
			guiStyleColor.active.textColor = Color.white;
			guiStyleColor.fixedHeight = 60;
			guiStyleColor.fixedWidth = 60;
		}
	}

	void fillWalls(MapCellData mcd) {

		//CANTO SUPERIOR ESQUERDO
		if (mcd.limites[0] == Limite.CONEXAO && mcd.limites[1] == Limite.CONEXAO)
			mcd.blueprint[0, 0] = Tile.CHAO;
		else if (mcd.limites[0] == Limite.CONEXAO)
			mcd.blueprint[0, 0] = Tile.PAREDE_O;
		else if (mcd.limites[1] == Limite.CONEXAO)
			mcd.blueprint[0, 0] = Tile.PAREDE_N;
		else
			mcd.blueprint[0, 0] = Tile.PAREDE_NO;

		//CANTO SUPERIOR DIREITO
		if (mcd.limites[0] == Limite.CONEXAO && mcd.limites[2] == Limite.CONEXAO)
			mcd.blueprint[0, mcd.dimensao - 1] = Tile.CHAO;
		else if (mcd.limites[0] == Limite.CONEXAO)
			mcd.blueprint[0, mcd.dimensao - 1] = Tile.PAREDE_L;
		else if (mcd.limites[2] == Limite.CONEXAO)
			mcd.blueprint[0, mcd.dimensao - 1] = Tile.PAREDE_N;
		else
			mcd.blueprint[0, mcd.dimensao - 1] = Tile.PAREDE_NL;

		//CANTO INFERIOR ESQUERDO
		if (mcd.limites[1] == Limite.CONEXAO && mcd.limites[3] == Limite.CONEXAO)
			mcd.blueprint[mcd.dimensao - 1, 0] = Tile.CHAO;
		else if (mcd.limites[1] == Limite.CONEXAO)
			mcd.blueprint[mcd.dimensao - 1, 0] = Tile.PAREDE_S;
		else if (mcd.limites[3] == Limite.CONEXAO)
			mcd.blueprint[mcd.dimensao - 1, 0] = Tile.PAREDE_O;
		else
			mcd.blueprint[mcd.dimensao - 1, 0] = Tile.PAREDE_SO;

		//CANTO INFERIOR DIREITO
		if (mcd.limites[2] == Limite.CONEXAO && mcd.limites[3] == Limite.CONEXAO)
			mcd.blueprint[mcd.dimensao - 1, mcd.dimensao - 1] = Tile.CHAO;
		else if (mcd.limites[2] == Limite.CONEXAO)
			mcd.blueprint[mcd.dimensao - 1, mcd.dimensao - 1] = Tile.PAREDE_S;
		else if (mcd.limites[3] == Limite.CONEXAO)
			mcd.blueprint[mcd.dimensao - 1, mcd.dimensao - 1] = Tile.PAREDE_L;
		else
			mcd.blueprint[mcd.dimensao - 1, mcd.dimensao - 1] = Tile.PAREDE_SL;
		
		//PAREDES RESTANTES
		for (int i = 1; i < mcd.dimensao - 1; i++) {
			mcd.blueprint[i, 0] = (mcd.limites[1] != Limite.CONEXAO) ? Tile.PAREDE_O : Tile.CHAO;
			mcd.blueprint[i, mcd.dimensao - 1] = (mcd.limites[2] != Limite.CONEXAO) ? Tile.PAREDE_L : Tile.CHAO;
			mcd.blueprint[0, i] = (mcd.limites[0] != Limite.CONEXAO) ? Tile.PAREDE_N : Tile.CHAO;
			mcd.blueprint[mcd.dimensao - 1, i] = (mcd.limites[3] != Limite.CONEXAO) ? Tile.PAREDE_S : Tile.CHAO;
		}

		//PORTAS
		if (mcd.limites[0] == Limite.PORTA) mcd.blueprint[0, mcd.dimensao / 2] = Tile.PORTA;
		if (mcd.limites[1] == Limite.PORTA) mcd.blueprint[mcd.dimensao / 2, 0] = Tile.PORTA;
		if (mcd.limites[2] == Limite.PORTA) mcd.blueprint[mcd.dimensao / 2, mcd.dimensao - 1] = Tile.PORTA;
		if (mcd.limites[3] == Limite.PORTA) mcd.blueprint[mcd.dimensao - 1, mcd.dimensao / 2] = Tile.PORTA;
	}

	void fillFloor(MapCellData mcd) {
		for (int i = 1; i < mcd.dimensao - 1; i++) 
			for (int j = 1; j < mcd.dimensao - 1; j++)
				mcd.blueprint[i, j] = Tile.CHAO;
		fillWalls(mcd);
	}

	public override void OnInspectorGUI() {
		initializeGuiStyleColor();
		if (colors == null) colors = new Colors();

		MapCellData mcd = (MapCellData)target;

		if (mcd.limites == null) mcd.limites = new Limite[4];
		if (mcd.blueprint == null) {
			mcd.blueprint = new Tile[mcd.dimensao, mcd.dimensao];
			fillFloor(mcd);
		}

		//Centralizar norte/sul
		GUIStyle centerStyle = new GUIStyle();
		centerStyle.margin = new RectOffset(75,0,0,0);

		EditorGUILayout.LabelField("Limites", EditorStyles.boldLabel);
		GUILayout.BeginVertical(centerStyle, GUILayout.Width(100));
		mcd.limites[0] = (Limite) EditorGUILayout.EnumPopup(mcd.limites[0]);
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUILayout.Width(200));
		GUILayout.BeginHorizontal();
		mcd.limites[1] = (Limite)EditorGUILayout.EnumPopup(mcd.limites[1]);
		mcd.limites[2] = (Limite)EditorGUILayout.EnumPopup(mcd.limites[2]);
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.BeginVertical(centerStyle, GUILayout.Width(100));
		mcd.limites[3] = (Limite)EditorGUILayout.EnumPopup(mcd.limites[3]);
		GUILayout.EndVertical();

		EditorGUILayout.Space();
		GUILayout.BeginVertical(GUILayout.Width(400));
		if (GUILayout.Button("Generate walls and doors")) {
			fillWalls(mcd);
		}
		GUILayout.EndVertical();
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Blueprint", EditorStyles.boldLabel);
		GUILayout.BeginVertical("Box");

		for(int i = 0; i<mcd.dimensao; i++) {
			GUILayout.BeginHorizontal();
			for (int j = 0; j<mcd.dimensao; j++) {
				switch (mcd.blueprint[i, j]) {
					case Tile.VAZIO:
						GUI.backgroundColor = colors.vazio;
						mcd.blueprint[i, j] = (Tile)EditorGUILayout.EnumPopup(mcd.blueprint[i, j], guiStyleColor, GUILayout.ExpandHeight(true));
						break;
					case Tile.CHAO:
						GUI.backgroundColor = colors.chao;
						mcd.blueprint[i, j] = (Tile)EditorGUILayout.EnumPopup(mcd.blueprint[i, j], guiStyleColor, GUILayout.ExpandHeight(true));
						break;
					case Tile.PORTA:
						GUI.backgroundColor = colors.porta;
						mcd.blueprint[i, j] = (Tile)EditorGUILayout.EnumPopup(mcd.blueprint[i, j], guiStyleColor, GUILayout.ExpandHeight(true));
						break;
					case Tile.LAVA:
						GUI.backgroundColor = colors.lava;
						mcd.blueprint[i, j] = (Tile)EditorGUILayout.EnumPopup(mcd.blueprint[i, j], guiStyleColor, GUILayout.ExpandHeight(true));
						break;
					case Tile.ESPINHO:
						GUI.backgroundColor = colors.espinho;
						mcd.blueprint[i, j] = (Tile)EditorGUILayout.EnumPopup(mcd.blueprint[i, j], guiStyleColor, GUILayout.ExpandHeight(true));
						break;
					default:
						GUI.backgroundColor = colors.parede;
						mcd.blueprint[i, j] = (Tile)EditorGUILayout.EnumPopup(mcd.blueprint[i, j], guiStyleColor, GUILayout.ExpandHeight(true));
						break;
				}
			}
			GUILayout.EndHorizontal();
		}

		GUILayout.EndVertical();
	}

}
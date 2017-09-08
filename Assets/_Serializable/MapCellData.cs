﻿using System.Collections;
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

    public int dimension_x = 15;
    public int dimension_y = 15;

    public Tile[] blueprint;
	public Limite[] limites;
	public EnemySpawnerData[] esd;

    public int accessMatrix(int x, int y) {
        return x * dimension_y + y;
    }
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

    private int new_x = 0, new_y = 0;

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

    void setDimensions(MapCellData mcd, int x, int y) {
        if (mcd.dimension_x == x && mcd.dimension_y == y) return;

        mcd.dimension_x = x;
        mcd.dimension_y = y;
        mcd.blueprint = new Tile[mcd.dimension_x * mcd.dimension_y];
        fillFloor(mcd);
    }

	void fillWalls(MapCellData mcd) {

		//CANTO SUPERIOR ESQUERDO
		if (mcd.limites[0] == Limite.CONEXAO && mcd.limites[1] == Limite.CONEXAO)
			mcd.blueprint[mcd.accessMatrix(0, 0)] = Tile.CHAO;
		else if (mcd.limites[0] == Limite.CONEXAO)
			mcd.blueprint[mcd.accessMatrix(0, 0)] = Tile.PAREDE_O;
		else if (mcd.limites[1] == Limite.CONEXAO)
			mcd.blueprint[mcd.accessMatrix(0, 0)] = Tile.PAREDE_N;
		else
			mcd.blueprint[mcd.accessMatrix(0, 0)] = Tile.PAREDE_NO;

		//CANTO SUPERIOR DIREITO
		if (mcd.limites[0] == Limite.CONEXAO && mcd.limites[2] == Limite.CONEXAO)
			mcd.blueprint[mcd.accessMatrix(0, mcd.dimension_y - 1)] = Tile.CHAO;
		else if (mcd.limites[0] == Limite.CONEXAO)
			mcd.blueprint[mcd.accessMatrix(0, mcd.dimension_y - 1)] = Tile.PAREDE_L;
		else if (mcd.limites[2] == Limite.CONEXAO)
			mcd.blueprint[mcd.accessMatrix(0, mcd.dimension_y - 1)] = Tile.PAREDE_N;
		else
			mcd.blueprint[mcd.accessMatrix(0, mcd.dimension_y - 1)] = Tile.PAREDE_NL;

		//CANTO INFERIOR ESQUERDO
		if (mcd.limites[1] == Limite.CONEXAO && mcd.limites[3] == Limite.CONEXAO)
			mcd.blueprint[mcd.accessMatrix(mcd.dimension_x - 1, 0)] = Tile.CHAO;
		else if (mcd.limites[1] == Limite.CONEXAO)
			mcd.blueprint[mcd.accessMatrix(mcd.dimension_x - 1, 0)] = Tile.PAREDE_S;
		else if (mcd.limites[3] == Limite.CONEXAO)
			mcd.blueprint[mcd.accessMatrix(mcd.dimension_x - 1, 0)] = Tile.PAREDE_O;
		else
			mcd.blueprint[mcd.accessMatrix(mcd.dimension_x - 1, 0)] = Tile.PAREDE_SO;

		//CANTO INFERIOR DIREITO
		if (mcd.limites[2] == Limite.CONEXAO && mcd.limites[3] == Limite.CONEXAO)
			mcd.blueprint[mcd.accessMatrix(mcd.dimension_x - 1, mcd.dimension_y - 1)] = Tile.CHAO;
		else if (mcd.limites[2] == Limite.CONEXAO)
			mcd.blueprint[mcd.accessMatrix(mcd.dimension_x - 1, mcd.dimension_y - 1)] = Tile.PAREDE_S;
		else if (mcd.limites[3] == Limite.CONEXAO)
			mcd.blueprint[mcd.accessMatrix(mcd.dimension_x - 1, mcd.dimension_y - 1)] = Tile.PAREDE_L;
		else
			mcd.blueprint[mcd.accessMatrix(mcd.dimension_x - 1, mcd.dimension_y - 1)] = Tile.PAREDE_SL;

        //PAREDES RESTANTES
        for (int i = 1; i < mcd.dimension_x - 1; i++) {
            mcd.blueprint[mcd.accessMatrix(i, 0)] = (mcd.limites[1] != Limite.CONEXAO) ? Tile.PAREDE_O : Tile.CHAO;
            mcd.blueprint[mcd.accessMatrix(i, mcd.dimension_y - 1)] = (mcd.limites[2] != Limite.CONEXAO) ? Tile.PAREDE_L : Tile.CHAO;
        }
        for (int i = 1; i < mcd.dimension_y - 1; i++) {
            mcd.blueprint[mcd.accessMatrix(0, i)] = (mcd.limites[0] != Limite.CONEXAO) ? Tile.PAREDE_N : Tile.CHAO;
            mcd.blueprint[mcd.accessMatrix(mcd.dimension_x - 1, i)] = (mcd.limites[3] != Limite.CONEXAO) ? Tile.PAREDE_S : Tile.CHAO;
        }

        //PORTAS
        if (mcd.limites[0] == Limite.PORTA) mcd.blueprint[mcd.accessMatrix(0, mcd.dimension_y / 2)] = Tile.PORTA;
		if (mcd.limites[1] == Limite.PORTA) mcd.blueprint[mcd.accessMatrix(mcd.dimension_x / 2, 0)] = Tile.PORTA;
		if (mcd.limites[2] == Limite.PORTA) mcd.blueprint[mcd.accessMatrix(mcd.dimension_x / 2, mcd.dimension_y - 1)] = Tile.PORTA;
		if (mcd.limites[3] == Limite.PORTA) mcd.blueprint[mcd.accessMatrix(mcd.dimension_x - 1, mcd.dimension_y / 2)] = Tile.PORTA;
	}

	void fillFloor(MapCellData mcd) {
		for (int i = 1; i < mcd.dimension_x - 1; i++) 
			for (int j = 1; j < mcd.dimension_y - 1; j++)
				mcd.blueprint[mcd.accessMatrix(i, j)] = Tile.CHAO;
		fillWalls(mcd);
	}

	public override void OnInspectorGUI() {
        EditorGUI.BeginChangeCheck();
        Color defaultColor = GUI.backgroundColor;

        initializeGuiStyleColor();
		if (colors == null) colors = new Colors();

		MapCellData mcd = (MapCellData)target;

        if (mcd.limites == null) mcd.limites = new Limite[4];
		if (mcd.blueprint == null) {
			mcd.blueprint = new Tile[mcd.dimension_x * mcd.dimension_y];
			fillFloor(mcd);
		}
        //Centralizar norte/sul
        GUIStyle centerStyle = new GUIStyle();
		centerStyle.margin = new RectOffset(57,0,0,0);

		EditorGUILayout.LabelField("Limites", EditorStyles.boldLabel);
        GUILayout.BeginVertical("Box");
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
        GUILayout.EndVertical();

        //Botao de modificar dimensoes
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Dimensões", EditorStyles.boldLabel);
        GUILayout.BeginVertical("Box", GUILayout.Width(200));
        GUILayout.BeginHorizontal();

        if (new_x < 1) new_x = mcd.dimension_x;
        GUILayout.Label(new_x + string.Empty, GUILayout.Width(30));
        new_x = Mathf.RoundToInt(GUILayout.HorizontalSlider(new_x, 1, 31));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (new_y < 1) new_y = mcd.dimension_y;
        GUILayout.Label(new_y + string.Empty, GUILayout.Width(30));
        new_y = Mathf.RoundToInt(GUILayout.HorizontalSlider(new_y, 1, 31));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = new Color(1f, .3f, .3f);
        if (GUILayout.Button(new GUIContent("Change dimensions", "Changing dimensions will reset the map."), GUILayout.Height(30))) {
            setDimensions(mcd, new_x, new_y);
        }
        GUI.backgroundColor = defaultColor;
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        EditorGUILayout.Space();

        //Botao de Geracao de Paredes
        EditorGUILayout.Space();
		GUILayout.BeginVertical(GUILayout.Width(400));
		if (GUILayout.Button(new GUIContent("Generate walls and doors", "This will generate walls and doors based on the limits set. All tiles on the borders will be overwritten."), GUILayout.Height(50))) {
			fillWalls(mcd);
        }
		GUILayout.EndVertical();
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Blueprint", EditorStyles.boldLabel);
		GUILayout.BeginVertical("Box");

        //Centralizar indices
        centerStyle = new GUIStyle("Label");
        centerStyle.alignment = TextAnchor.LowerCenter;

        GUILayout.BeginHorizontal();
        for (int j = 0; j < mcd.dimension_y; j++) GUILayout.Label(j.ToString("D2") + string.Empty, centerStyle);
        GUILayout.EndHorizontal();
        for (int i = 0; i<mcd.dimension_x; i++) {
			GUILayout.BeginHorizontal();
            GUILayout.Label(i.ToString("D2") + string.Empty, centerStyle);
			for (int j = 0; j<mcd.dimension_y; j++) {
                switch (mcd.blueprint[mcd.accessMatrix(i, j)]) {
					case Tile.VAZIO:
						GUI.backgroundColor = colors.vazio;
                        mcd.blueprint[mcd.accessMatrix(i, j)] = (Tile)EditorGUILayout.EnumPopup(mcd.blueprint[mcd.accessMatrix(i, j)], guiStyleColor, GUILayout.ExpandHeight(true));
						break;
					case Tile.CHAO:
						GUI.backgroundColor = colors.chao;
						mcd.blueprint[mcd.accessMatrix(i, j)] = (Tile)EditorGUILayout.EnumPopup(mcd.blueprint[mcd.accessMatrix(i, j)], guiStyleColor, GUILayout.ExpandHeight(true));
						break;
					case Tile.PORTA:
						GUI.backgroundColor = colors.porta;
						mcd.blueprint[mcd.accessMatrix(i, j)] = (Tile)EditorGUILayout.EnumPopup(mcd.blueprint[mcd.accessMatrix(i, j)], guiStyleColor, GUILayout.ExpandHeight(true));
						break;
					case Tile.LAVA:
						GUI.backgroundColor = colors.lava;
						mcd.blueprint[mcd.accessMatrix(i, j)] = (Tile)EditorGUILayout.EnumPopup(mcd.blueprint[mcd.accessMatrix(i, j)], guiStyleColor, GUILayout.ExpandHeight(true));
						break;
					case Tile.ESPINHO:
						GUI.backgroundColor = colors.espinho;
						mcd.blueprint[mcd.accessMatrix(i, j)] = (Tile)EditorGUILayout.EnumPopup(mcd.blueprint[mcd.accessMatrix(i, j)], guiStyleColor, GUILayout.ExpandHeight(true));
						break;
					default:
						GUI.backgroundColor = colors.parede;
						mcd.blueprint[mcd.accessMatrix(i, j)] = (Tile)EditorGUILayout.EnumPopup(mcd.blueprint[mcd.accessMatrix(i, j)], guiStyleColor, GUILayout.ExpandHeight(true));
						break;
                }
            }
			GUILayout.EndHorizontal();
        }

		GUILayout.EndVertical();
        
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(mcd);
            //Undo.RecordObject(mcd, "Modified Map Cell \"" + mcd.name + "\"");
            //Undo.FlushUndoRecordObjects();
        }
    }

}
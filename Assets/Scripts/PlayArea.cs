using UnityEngine;
using System.Collections;

public class PlayArea : MonoBehaviour {

	int GRID_SIZE_X = 16;
	int GRID_SIZE_Y = 12;
	State[,] grid;


	public enum State {
		Empty,
		Wired,
		Ground,
		Ladder
	}



	// Use this for initialization
	void Start () {
		grid = new State[GRID_SIZE_X, GRID_SIZE_Y];
		for(int y = 0; y < GRID_SIZE_Y; y++){
			for(int x  = 0; x < GRID_SIZE_X; x++){
				grid[x, y] = State.Empty;

			}
		}
	}



	public void ChangeState(State state, IntVector2 block){

		if(block.x < GRID_SIZE_X && block.y < GRID_SIZE_Y && grid[block.x, block.y] != null){
			grid[block.x, block.y] = state;
		}else{
			Debug.LogError("Cannot change state because" +block.ToString()+" doesn't exist in grid");

		}
	}


}

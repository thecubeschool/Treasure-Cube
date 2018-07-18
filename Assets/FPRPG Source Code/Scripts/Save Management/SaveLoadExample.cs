using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using FRPG.SMSaveManager;

#pragma warning disable 0219

public class SaveLoadExample : MonoBehaviour {
#pragma warning restore 1030 // #warning directive

    //Here it is shown how to use save/load system

    private void Example() {

        //First we start by creating our save manager file
        //Then we pass a name that our save will be using and password for encryption
        //By default save manager will use Applocation.persistentDataPath for storing the file
        SMSaveManager toSave = new SMSaveManager("SaveGameName.sav", "abc123");

        //If we want to set custom folder where save files will be store we can do it like this:
        string savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\FPS Source Code\SaveGameName.sav";
        toSave = new SMSaveManager(new FileInfo(savePath), "abc123");

        //We can now access our save file and open it to store in it our data
        toSave.Open();

        //Now we can save our data with:
        toSave.SetValue("NameOfOurData_Int", 0);
        toSave.SetValue("NameOfOurData_Float", 0f);
        toSave.SetValue("NameOfOurData_Boolean", false);
        toSave.SetValue("NameOfOurData_String", "Jack");
        toSave.SetValue("NameOfOurData_Vector2", new Vector2(0f, 0f));
        toSave.SetValue("NameOfOurData_Vector3", new Vector3(0f, 0f, 0f));

        //And if we want to finish it at the end we need to add
        toSave.Save();

        //If we want to load we need to open the saveFile like shown above with:
        SMSaveManager toLoad = new SMSaveManager("SaveGameName.sav", "abc123"); //Or from custom folder

        toLoad.Open();

        int testInt = toLoad.GetValue<int>("NameOfOurData_Int", 0);
        float testFloat = toLoad.GetValue<float>("NameOfOurData_Float", 0f);
        bool testBool = toLoad.GetValue<bool>("NameOfOurData_Boolean", false);
        string testString = toLoad.GetValue<string>("NameOfOurData_String", string.Empty);
        Vector2 testVector2 = toLoad.GetValue<Vector2>("NameOfOurData_Vector2", Vector2.zero);
        Vector3 testVector3 = toLoad.GetValue<Vector3>("NameOfOurData_Vector3", Vector3.zero);

        //Above we can see that we are loading our data by using same naming as we used when saving them, and also in brackets we are setting
        //default value that should be if in some case there is no data saved by given name

        //And in the end we are closing our save file
        toLoad.Save();
    }

    //Now to save our player/world etc we need to use all this with a all new ID system that has been added to the source
    //Every npc and item have its own unique id that we use to distinguish one from another and to save/load their current in game state
    //You can see that in SaveLoad.cs script
}

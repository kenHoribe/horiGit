package com.example.hogeroid001;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        testFunc01();
    }

    private void testFunc01(){
        Log.d("MyApp", "testFunc01 start");
        Log.d("MyApp", "testFunc01 end");
    }
}
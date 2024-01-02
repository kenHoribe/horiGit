package com.example.section0901;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;

import java.io.BufferedWriter;
import java.io.IOException;
import java.io.OutputStreamWriter;

public class MainActivity extends AppCompatActivity {

    EditText txtMemo;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        this.txtMemo = findViewById(R.id.txtMemo);
    }

    public void btnSave_onClick(View view)
    {
        //memo.dat への書き込みを準備
        try(BufferedWriter write = new BufferedWriter( new OutputStreamWriter(
                openFileOutput("memo.dat", Context.MODE_PRIVATE)))) {
            //EditTextへの入力値をファイルに書き込み
            write.write(txtMemo.getText().toString());
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
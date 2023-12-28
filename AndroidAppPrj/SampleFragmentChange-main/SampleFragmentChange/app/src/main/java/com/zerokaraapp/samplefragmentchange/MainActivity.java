package com.zerokaraapp.samplefragmentchange;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //MainActivityが強制終了された時は、savedInstanceStateにFragmentの情報が入ってonCreateが呼び出される
        //最初の1回だけフラグメントを作成するため、nullで条件分岐
        if (savedInstanceState == null) {
            //FragmentManagerのインスタンスを取得し、FragmentTransactionを開始
            getSupportFragmentManager().beginTransaction()
                    //トランザクションに関与するフラグメントの状態変更を最適化
                    .setReorderingAllowed(true)
                    //フラグメントを追加するよう指示。第3引数はデータを受け渡す場合に使用
                    .add(R.id.fragmentContainerView, Fragment1.class, null)
                    //上記のトランザクションに設定した内容を実行
                    .commit();
        }

        //レイアウトからボタンオブジェクトをそれぞれ取得
        Button button1 = findViewById(R.id.button1);
        Button button2 = findViewById(R.id.button2);

        //ボタンオブジェクトににリスナをそれぞれ設定
        button1.setOnClickListener(new ButtonClickListener());
        button2.setOnClickListener(new ButtonClickListener());
    }

    //ボタンを押したときの動作を定義するリスナクラス
    private class ButtonClickListener implements View.OnClickListener{
        //ボタンを押したときの動作を定義
        @Override
        public void onClick(View view){
            //ボタンのIDによって条件分岐
            switch(view.getId()){
                //ボタン1が押された場合
                case R.id.button1:
                    //FragmentManagerのインスタンスを取得し、FragmentTransactionを開始
                    getSupportFragmentManager().beginTransaction()
                            //トランザクションに関与するフラグメントの状態変更を最適化
                            .setReorderingAllowed(true)
                            //フラグメント1に入れ替えするよう指示。第3引数はデータを受け渡す場合に使用
                            .replace(R.id.fragmentContainerView, Fragment1.class, null)
                            //上記のトランザクションに設定した内容を実行
                            .commit();
                    break;

                //ボタン2が押された場合
                case R.id.button2:
                    //FragmentManagerのインスタンスを取得し、FragmentTransactionを開始
                    getSupportFragmentManager().beginTransaction()
                            //トランザクションに関与するフラグメントの状態変更を最適化
                            .setReorderingAllowed(true)
                            //フラグメント2に入れ替えするよう指示。第3引数はデータを受け渡す場合に使用
                            .replace(R.id.fragmentContainerView, Fragment2.class, null)
                            //上記のトランザクションに設定した内容を実行
                            .commit();
                    break;
                default:
                    break;
            }
        }
    }
}
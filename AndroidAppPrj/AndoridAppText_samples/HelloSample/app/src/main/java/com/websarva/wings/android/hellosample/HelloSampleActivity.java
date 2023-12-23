package com.websarva.wings.android.hellosample;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

/**
 * 『Androidアプリ開発の教科書』
 * 第4章
 * イベントとリスナサンプル
 *
 * メインアクティビティクラス。
 *
 * @author Shinzo SAITO
 */
public class HelloSampleActivity extends AppCompatActivity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_hello_sample);

		//表示ボタンであるButtonオブジェクトを取得。
		Button btClick = findViewById(R.id.btClick);
		//リスナクラスのインスタンスを生成。
		HelloListener listener = new HelloListener();
		//表示ボタンにリスナを設定。
		btClick.setOnClickListener(listener);

		//クリアボタンであるButtonオブジェクトを取得。
		Button btClear = findViewById(R.id.btClear);
		//クリアボタンにリスナを設定。
		btClear.setOnClickListener(listener);
	}

	/**
	 * ボタンをクリックしたときのリスナクラス。
	 */
	private class HelloListener implements View.OnClickListener {

		@Override
		public void onClick(View view) {
			//名前入力欄であるEditTextオブジェクトを取得。
			EditText input = findViewById(R.id.etName);
			//メッセージを表示するTextViewオブジェクトを取得。
			TextView output = findViewById(R.id.tvOutput);

			//タップされた画面部品のidのR値を取得。
			int id = view.getId();
			//idのR値に応じて処理を分岐。
			switch(id) {
				//表示ボタンの場合…
				case R.id.btClick:
					//入力された名前文字列を取得。
					String inputStr = input.getText().toString();
					//メッセージを表示。
					output.setText(inputStr + "さん、こんにちは!");
					break;
				//クリアボタンの場合…
				case R.id.btClear:
					//名前入力欄を空文字に設定。
					input.setText("");
					//メッセージ表示欄を空文字に設定。
					output.setText("");
					break;
			}
		}
	}
}

package com.websarva.wings.android.cameraintentsample;

import android.Manifest;
import android.content.ContentResolver;
import android.content.ContentValues;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.provider.MediaStore;
import android.support.v4.app.ActivityCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;

import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * 『Androidアプリ開発の教科書』
 * 第15章
 * カメラアプリ連携サンプル
 *
 * アクティビティクラス。
 *
 * @author Shinzo SAITO
 */
public class UseCameraActivity extends AppCompatActivity {
	/**
	 * 保存された画像のURI。
	 */
	private Uri _imageUri;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_use_camera);
	}

	@Override
	public void onActivityResult(int requestCode, int resultCode, Intent data) {
		//カメラアプリからの戻りでかつ撮影成功の場合
		if(requestCode == 200 && resultCode == RESULT_OK) {
			//画像を表示するImageViewを取得。
			ImageView ivCamera = findViewById(R.id.ivCamera);
			//フィールドの画像URIをImageViewに設定。
			ivCamera.setImageURI(_imageUri);
		}
	}

	@Override
	public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
		//WRITE_EXTERNAL_STORAGEに対するパーミションダイアログでかつ許可を選択したなら…
		if(requestCode == 2000 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
			//もう一度カメラアプリを起動。
			ImageView ivCamera = findViewById(R.id.ivCamera);
			onCameraImageClick(ivCamera);
		}
	}

	/**
	 * 画像部分がタップされたときの処理メソッド。
	 */
	public void onCameraImageClick(View view) {
		//WRITE_EXTERNAL_STORAGEの許可が下りていないなら…
		if(ActivityCompat.checkSelfPermission(this, Manifest.permission.WRITE_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED) {
			//WRITE_EXTERNAL_STORAGEの許可を求めるダイアログを表示。その際、リクエストコードを2000に設定。
			String[] permissions = {Manifest.permission.WRITE_EXTERNAL_STORAGE};
			ActivityCompat.requestPermissions(this, permissions, 2000);
			return;
		}

		//日時データを「yyyyMMddHHmmss」の形式に整形するフォーマッタを生成。
		SimpleDateFormat dateFormat = new SimpleDateFormat("yyyyMMddHHmmss");
		//現在の日時を取得。
		Date now = new Date(System.currentTimeMillis());
		//取得した日時データを「yyyyMMddHHmmss」形式に整形した文字列を生成。
		String nowStr = dateFormat.format(now);
		//ストレージに格納する画像のファイル名を生成。ファイル名の一意を確保するためにタイムスタンプの値を利用。
		String fileName = "UseCameraActivityPhoto_" + nowStr +".jpg";

		//ContentValuesオブジェクトを生成。
		ContentValues values = new ContentValues();
		//画像ファイル名を設定。
		values.put(MediaStore.Images.Media.TITLE, fileName);
		//画像ファイルの種類を設定。
		values.put(MediaStore.Images.Media.MIME_TYPE, "image/jpeg");

		//ContentResolverオブジェクトを生成。
		ContentResolver resolver = getContentResolver();
		//ContentResolverを使ってURIオブジェクトを生成。
		_imageUri = resolver.insert(MediaStore.Images.Media.EXTERNAL_CONTENT_URI, values);
		//Intentオブジェクトを生成。
		Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
		//Extra情報として_imageUriを設定。
		intent.putExtra(MediaStore.EXTRA_OUTPUT, _imageUri);
		//アクティビティを起動。
		startActivityForResult(intent, 200);
	}
}

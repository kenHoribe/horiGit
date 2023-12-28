package to.msn.wings.listmyadapter;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.ListView;

import java.util.ArrayList;
import java.util.Random;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        String[] titles = { "革命のエチュード", "G線上のアリア", "シャコンヌ", "夜の女王のアリア", "春の海" };
        String[] tags = { "ピアノ", "バイオリン", "チェロ", "声楽", "箏" };
        String[] descs = { "ピアノの詩人と言われたショパンの代表的なピアノ曲です。",
            "バッハの作品。バイオリンのG線のみで演奏できることからこのタイトルで親しまれています。",
            "バッハの作品。パルティータ第2番の終曲です。", "モーツァルト作曲のオペラ「魔笛」の中のアリアです。",
            "宮城道雄の作品です。曲の舞台は鞆の浦と言われています。" };
        ArrayList<ListItem> data = new ArrayList<>();
        for (int i = 0; i < titles.length; i++) {
            ListItem item = new ListItem();
            item.setId((new Random()).nextLong());
            item.setTitle(titles[i]);
            item.setTag(tags[i]);
            item.setDesc(descs[i]);
            data.add(item);
        }
        MyListAdapter adapter = new MyListAdapter(this, data, R.layout.list_item);
        ListView list = findViewById(R.id.list);
        list.setAdapter(adapter);
    }
}

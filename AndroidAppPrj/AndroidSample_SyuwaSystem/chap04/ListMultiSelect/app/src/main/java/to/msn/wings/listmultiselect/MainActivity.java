package to.msn.wings.listmultiselect;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.CheckedTextView;
import android.widget.ListView;
import android.widget.Toast;

import java.util.ArrayList;

public class MainActivity extends AppCompatActivity {
    ArrayAdapter<String> adapter;
    ListView list;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        final ArrayList<String> data = new ArrayList<>();
        data.add("胡椒");
        data.add("ターメリック");
        data.add("コリアンダー");
        data.add("生姜");
        data.add("ニンニク");
        data.add("サフラン");

        adapter = new ArrayAdapter<>(
                this, android.R.layout.simple_list_item_multiple_choice, data);
        list = findViewById(R.id.list);
        list.setAdapter(adapter);
        list.setOnItemClickListener(
                new AdapterView.OnItemClickListener() {
                    public void onItemClick(AdapterView<?> av,
                                            View view, int position, long id) {
                        StringBuilder msg = new StringBuilder("選択したのは、");
                        for (int i = 0; i < list.getChildCount(); i++) {
                            CheckedTextView check = (CheckedTextView) list.getChildAt(i);
                            if (check.isChecked()) {
                                msg.append(check.getText()).append(",");
                            }
                        }
                        Toast.makeText(MainActivity.this, msg.substring(0, msg.length() - 1), Toast.LENGTH_LONG).show();

                    }
                }
        );
    }
}

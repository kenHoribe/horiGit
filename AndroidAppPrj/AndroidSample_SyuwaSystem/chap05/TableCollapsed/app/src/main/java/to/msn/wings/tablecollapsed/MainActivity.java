package to.msn.wings.tablecollapsed;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.TableLayout;

public class MainActivity extends AppCompatActivity {
    private boolean flag = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    public void btnflag_onClick(View view) {
        TableLayout tbl = findViewById(R.id.tbl);
        tbl.setColumnCollapsed(1, flag);
        flag = !flag;
    }
}

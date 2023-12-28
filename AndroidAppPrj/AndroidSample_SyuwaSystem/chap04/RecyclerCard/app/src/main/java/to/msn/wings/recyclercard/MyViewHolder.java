package to.msn.wings.recyclercard;

import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.TextView;

class MyViewHolder extends RecyclerView.ViewHolder {
    TextView title;
    TextView tag;
    TextView desc;

    MyViewHolder(View itemView) {
        super(itemView);
        this.title = itemView.findViewById(R.id.title);
        this.tag = itemView.findViewById(R.id.tag);
        this.desc = itemView.findViewById(R.id.desc);
    }
}

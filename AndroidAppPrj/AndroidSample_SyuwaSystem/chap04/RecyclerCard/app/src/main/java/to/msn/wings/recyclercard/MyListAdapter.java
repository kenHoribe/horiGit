package to.msn.wings.recyclercard;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import java.util.ArrayList;

public class MyListAdapter extends RecyclerView.Adapter<MyViewHolder> {
    private ArrayList<ListItem> data;

    public MyListAdapter(ArrayList<ListItem> data) {
        this.data = data;
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.list_item, parent, false);
        return new MyViewHolder(v);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        holder.title.setText(this.data.get(position).getTitle());
        holder.tag.setText(this.data.get(position).getTag());
        holder.desc.setText(this.data.get(position).getDesc());
    }

    @Override
    public int getItemCount() {
        return this.data.size();
    }
}

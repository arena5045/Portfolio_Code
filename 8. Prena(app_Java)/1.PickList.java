package com.example.Prena;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.res.TypedArray;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

public class PickList extends Fragment {

    String[] name;
    DBhelper dBhelper; //데이터베이스 객체
    SQLiteDatabase sqlDB; //sql 관리 객체
    Button reset;//데이터리셋버튼
    TextView dialog_text;//내용텍스트



    public static PickList newstance(){
        return new PickList();
    }

    GundamListAdaptar adapter;
    ListView listView;
    View diaView;//뷰


    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {


        final Intent intent = new Intent( getContext(), Gundam_pick_Info.class);

        View view  = inflater.inflate(R.layout.fragment_picklist,container,false);
        reset = view.findViewById(R.id.reset);
        adapter = new GundamListAdaptar();
        listView = (ListView) view.findViewById(R.id.listView);

        setData();

        listView.setAdapter(adapter);


        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {

                TextView a = view.findViewById(R.id.name);
               intent.putExtra("gundamname", a.getText().toString());
               startActivity(intent);
            }
        });

        dBhelper = new DBhelper(getContext());

        reset.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                AlertDialog.Builder dlg =new AlertDialog.Builder(getContext());
                diaView =View.inflate(getContext(),R.layout.warning_dialog,null);
                dlg.setView(diaView);//대화상자 설정
                dialog_text = diaView.findViewById(R.id.text_dialog);
                dialog_text.setText("정말로 찜목록을 초기화하겠습니까?");//대화상자 텍스트 설정

                dlg.setPositiveButton("초기화", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        resetData();
                        Toast.makeText(getContext(),  "찜 목록이 초기화 되었습니다", Toast.LENGTH_SHORT).show();

                    }
                }).setNegativeButton("취소", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        Toast.makeText(getContext(),  "취소되었습니다", Toast.LENGTH_SHORT).show();
                    }
                });
                dlg.show();

            }
        });




        return view;
    }


    public void resetData(){

        sqlDB = dBhelper.getWritableDatabase(); //writable로 데이터 베이스를 불러온다
        dBhelper.onUpgrade(sqlDB,0,1);//데이터베이스 class의 onUpgrade 매서드 호출
        sqlDB.close(); //데이터베이스 닫기
        Toast.makeText(getContext(),"찜목록 초기화 완료",Toast.LENGTH_SHORT).show();
        ((MainActivity)getActivity()).replaceFragment(PickList.newstance());

    }

    private void setData() {

        dBhelper = new DBhelper(getContext());
        sqlDB = dBhelper.getReadableDatabase();
        Cursor cursor; //커서 객체 생성
        cursor = sqlDB.rawQuery("SELECT * FROM PickGundam;",null); //커서에 담겨질 인자들을 SELECT문으로 불러옴



        TypedArray arrResId = getResources().obtainTypedArray(R.array.resId); //drawvle은 타입드 어레이에 저장해야한다.
        String[] titles = getResources().getStringArray(R.array.title);
        String[] contents = getResources().getStringArray(R.array.content);
        String[] scale = getResources().getStringArray(R.array.scale);
        name = new String[arrResId.length()];



        if (cursor.getCount() ==0) {//커서로 불러온 인자가 0개면
            Toast.makeText(getContext(), "찜한 프라모델이 없습니다!", Toast.LENGTH_SHORT).show(); // 등재된 자료가 없다고 토스트 메세지 호출
            return;//if문 나가기
        }

        while (cursor.moveToNext()){//moveToNext()는 다음값이 존재하면 true 아니면 false를 반환한다->커서에 담긴 인덱스만큼 반복
            int ID = cursor.getInt(0);//0번인덱스(이름)을 스트링 변수에 추가
            String memo = cursor.getString(1);//1번인덱스(인원)을 스트링 변수에 추가

            customDTO dto = new customDTO();
            dto.setResId(arrResId.getResourceId(ID, 0));
            dto.setTitle(titles[ID]);
            dto.setContent(memo);
            dto.setScale(scale[ID]);
            //name[ID] = titles[ID];

            adapter.addItem(dto);
        }

        cursor.close(); //커서 닫기
        sqlDB.close();//데이터베이스 닫기


    }


}
package com.example.Prena;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.res.TypedArray;
import android.database.sqlite.SQLiteDatabase;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.Nullable;

public class Gundam_pick_Info extends Activity {

    ImageView infoimage;//설명하는 이미지
    Button delete,changememo,close; //버튼 객체 생성
    DBhelper dBhelper; //데이터베이스 객체
    SQLiteDatabase sqlDB; //sql 관리 객체
    View diaView; // 뷰 객체
    EditText memo; // 메모 설정하는 에디트 텍스트
    TextView dialog_text; //대화상자 설명 텍스트
    String memo_first; // 처음 에딧텍스트에 들어갈 내용

    @Override
    protected void onDestroy() { //액티비티가 꺼질 때 호출
        super.onDestroy();
        ((MainActivity)MainActivity.staticContest).replaceFragment(PickList.newstance()); //메인액티비티의 프래그먼트 다시호출

    }

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.gundam_pickinfo);//프래그먼트에 뷰 세팅

        dBhelper = new DBhelper(this);

        delete = (Button)findViewById(R.id.pick_delete); //버튼 인플레이팅
        changememo = (Button)findViewById(R.id.memo_change);
        close = (Button)findViewById(R.id.close);
        infoimage = (ImageView)findViewById(R.id.infoimage);

        final String[] titles = getResources().getStringArray(R.array.title);//배열 인플레이팅
        String[] contents = getResources().getStringArray(R.array.content);

        Intent intent = getIntent();//인텐트
        String Gundam_name = intent.getStringExtra("gundamname");//이름값을 가져옴
        TypedArray arrInfo = getResources().obtainTypedArray(R.array.resInfo);

        for (int i = 0; i < titles.length; i++) {
            if (Gundam_name.equals(titles[i]) ) {//가져온 이름값과 타이틀을 비교
                //Toast.makeText(getApplicationContext(), i+"//"+titles[i] +"//"+ Gundam_name, Toast.LENGTH_SHORT).show(); 확인용 토스트메시지
                infoimage.setBackground((getDrawable(arrInfo.getResourceId(i, 0)))); // 이미지를 해당하는 이름으로 설정
                memo_first = contents[i]; //첫 에딧텍스트 텍스트 설정
            }

        }

        delete.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) { //삭제버튼 구현
                Intent intent = getIntent();
                final String Gundam_name = intent.getStringExtra("gundamname");
                //Toast.makeText(getApplicationContext(),  Gundam_name+"//"+titles[0], Toast.LENGTH_SHORT).show();

                AlertDialog.Builder dlg =new AlertDialog.Builder(Gundam_pick_Info.this);
                diaView =View.inflate(Gundam_pick_Info.this,R.layout.warning_dialog,null);
                dlg.setView(diaView);//대화상자 설정
                dialog_text = diaView.findViewById(R.id.text_dialog);
                dialog_text.setText("정말로 찜목록에서 제거하겠습니까?");//대화상자 텍스트 설정
                dlg.setPositiveButton("제거하기", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {

                        for (int i = 0; i < titles.length; i++) {
                            if (Gundam_name.equals(titles[i]) ) {


                                try {//오류를 검출해야 하는 부분을 try로 묶음
                                    sqlDB = dBhelper.getWritableDatabase(); //writable(쓰기)로 데이터 베이스를 불러온다
                                    sqlDB.execSQL("DELETE FROM PickGundam WHERE GundamID = ?", new  String[]{Integer.toString(i)});
                                    //딜리트 sql문 호출

                                    sqlDB.close();//데이터베이스 닫기
                                    Toast.makeText(getApplicationContext(),"찜 목록에서 삭제했습니다",Toast.LENGTH_SHORT).show();//
                                    finish(); // 액티비티 종료 ->온디스트로이 호출
                                }
                                catch (Exception e){//만약  Exception이 발생했을 경우
                                    Toast.makeText(getApplicationContext(),"Error! 데이터가 삭제되지 않았습니다! ",Toast.LENGTH_SHORT).show(); //에러토스트메세지를 호출한다.
                                }
                            }
                        }

                    }
                }).setNegativeButton("취소", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        Toast.makeText(getApplicationContext(),  "취소되었습니다", Toast.LENGTH_SHORT).show();
                    }
                });
                dlg.show();

            }
        });

        changememo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {//메모 변경 버튼 구현

                Intent intent = getIntent();
                final String Gundam_name = intent.getStringExtra("gundamname");

                AlertDialog.Builder dlg =new AlertDialog.Builder(Gundam_pick_Info.this);
                diaView =View.inflate(Gundam_pick_Info.this,R.layout.memo,null);
                dlg.setView(diaView);
                memo = diaView.findViewById(R.id.Edt1);
                memo.setText(memo_first);
                dlg.setPositiveButton("변경하기", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {


                        for (int i = 0; i < titles.length; i++) {
                            if (Gundam_name.equals(titles[i]) ) {


                                try {//오류를 검출해야 하는 부분을 try로 묶음
                                    sqlDB = dBhelper.getWritableDatabase(); //writable(쓰기)로 데이터 베이스를 불러온다
                                    sqlDB.execSQL("UPDATE PickGundam SET memo = ? WHERE GundamID = ? ", new  String[]{memo.getText().toString(),Integer.toString(i)});
                                    //sql 업데이트 구문

                                    sqlDB.close();//데이터베이스 닫기
                                    Toast.makeText(getApplicationContext(),"메모를 변경하였습니다",Toast.LENGTH_SHORT).show();//
                                    finish();
                                }
                                catch (Exception e){//만약  Exception이 발생했을 경우
                                    Toast.makeText(getApplicationContext(),"Error! 데이터가 변경되지 않았습니다! ",Toast.LENGTH_SHORT).show(); //에러토스트메세지를 호출한다.
                                }
                            }
                        }


                    }
                }).setNegativeButton("취소", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {//취소메세지
                        Toast.makeText(getApplicationContext(),  "취소되었습니다", Toast.LENGTH_SHORT).show();
                    }
                });


                dlg.show();//대화상자 표출
            }
        });

        close.setOnClickListener(new View.OnClickListener() {//닫기버튼
            @Override
            public void onClick(View v) {
                finish();
            }
        });





    }
}

package tmy.jack.mybluetoothplugin;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;

import android.bluetooth.BluetoothSocket;

import java.io.IOException;
import java.io.OutputStream;
import java.nio.charset.Charset;
import java.util.Set;
import java.util.UUID;

public class BluetoothManage {
    private static final UUID MY_UUID = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB");
    private BluetoothAdapter mBluetoothAdapter = null;
    private Set<BluetoothDevice> pairedDevices = null;
    private BluetoothDevice selectDevice = null;
    private BluetoothSocket mySocket = null;
    private OutputStream myOutputStream = null;

    // Bluetoothが使えるかどうかをかえす関数
    public boolean Enabled() {
        mBluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
        // 対応しているか
        if (mBluetoothAdapter == null) {
            return false;
        }
        // onになっているか
        if (mBluetoothAdapter.isEnabled()) {
            return true;
        } else {
            return  false;
        }
    }

    // ペアリング済みのBluetoothの名前とアドレスの一覧をstringで返す
    public String GetPairedDevices(){
        String result = "";
        // adapterが取れてなければとりあえずNULL文字
        if (mBluetoothAdapter == null) return result;
        // ぺアデバイス取得
        pairedDevices = mBluetoothAdapter.getBondedDevices();
        if (pairedDevices.size() > 0) {
            for (BluetoothDevice device : pairedDevices) {
                result += device.getName() + "\n" + device.getAddress() + "\n";
            }
        }
        return result;
    }

    // addressからデバイスを選択する
    // 対応デバイスが１つ(count == 1)の時のみ成功とする
    public boolean SelectDevice(String address){
        int count = 0;
        for (BluetoothDevice device : pairedDevices) {
            if (device.getAddress().equals(address)) {
                selectDevice = device;
                count++;
            }
        }
        if(count == 1) return true;
        else return false;
    }

    // socketを取得する関数
    public boolean GetSocket(){
        // デバイスが選択されてなければ無理
        if (selectDevice == null) return false;
        // socketの取得チャレンジ
        BluetoothSocket tmp = null;
        try {
            tmp = selectDevice.createRfcommSocketToServiceRecord(MY_UUID);
        } catch (IOException e) {
            e.printStackTrace();
            return false;
        }
        // tmpがnullの時は失敗のはず
        if (tmp == null)
            return false;
        else {
            mySocket = tmp;
            return  true;
        }
    }

    // 接続を試みる関数
    public boolean TryConnect(){
        // socketないと無理
        if (mySocket == null) return false;
        // 接続チャレンジ
        try {
            mySocket.connect();
        } catch (IOException e) {
            e.printStackTrace();
            // 失敗時はsocketを閉じる
            try {
                mySocket.close();
            } catch (IOException e2) {
                e2.printStackTrace();
            }
            return false;
        }
        // 例外発生してないから多分いけたでしょう
        return  true;
    }

    // outputStreamを取得する関数。
    public boolean GetOutputStream(){
        // socketないと無理
        if (mySocket == null) return false;
        // 取得チャレンジ
        OutputStream tmp = null;
        try {
            tmp = mySocket.getOutputStream();
        } catch (IOException e) {
            e.printStackTrace();
            return false;
        }
        // tmpがnullの時は失敗のはず
        if (tmp == null){
            return false;
        }
        else {
            myOutputStream = tmp;
            return true;
        }
    }

    // 文字列を送る関数
    public boolean SendString(String str){
        // outputStreamないと無理
        if (myOutputStream == null) return false;
        // 文字列をバイトに変換
        byte[] data = str.getBytes( Charset.forName("US-ASCII") );
        // 送信チャレンジ
        try {
            myOutputStream.write(data);
        } catch (IOException e) {
            e.printStackTrace();
            return false;
        }
        // 例外発生してないから多分いけたでしょう
        return true;
    }
}

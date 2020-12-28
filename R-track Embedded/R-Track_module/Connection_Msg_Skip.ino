void Check_bt_connection()
{
  if(use_bt_2[ID - 1]) Serial2.setTimeout(200);
  if(use_bt_4[ID - 1]) Serial4.setTimeout(200);

  String str_2 = "";
  String str_4 = "";
  bool bt_2_ready = true;
  bool bt_4_ready = true;

  if(use_bt_2[ID - 1]) bt_2_ready = false;
  if(use_bt_4[ID - 1]) bt_4_ready = false;


  while (true)
  {
    if (!bt_2_ready)
    {
      str_2 = Serial2.readString();
      if ((str_2 == "06") || (str_2 == "1TEST2TEST3TEST4TEST5TEST6TEST1TEST2TEST3TEST4TEST5TEST6TEST2TEST3TEST4TEST5TEST6TEST1TEST2TEST3TEST4TEST5TEST6TEST"))
      {
        bt_2_ready = true;
        Serial.println("BT_2 Ready!");
      }
    }

    if (!bt_4_ready)
    {
      str_4 = Serial4.readString();
      if ((str_4 == "06") || (str_4 == "1TEST2TEST3TEST4TEST5TEST6TEST1TEST2TEST3TEST4TEST5TEST6TEST2TEST3TEST4TEST5TEST6TEST1TEST2TEST3TEST4TEST5TEST6TEST"))
      {
        bt_4_ready = true;
        Serial.println("BT_4 Ready!");
      }
    }

    if (bt_2_ready && bt_4_ready) break;
    
    if(digitalRead(BDPIN_PUSH_SW_1) == true)
    {
      Serial.println("Bluetooth Connection Check Skipped!");
      break;
    }
  }

}

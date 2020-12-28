
//-----------------------------------값들 보내주는 함수들------------------------------------
void checkValueSend()
{

  currentMillis = millis();
  millisDiff = currentMillis - previousMillis;
  if (millisDiff >= valueSendDelay)
  {
    
    int curID = 0;
    previousMillis = currentMillis;

    for (int i = 0; i < 4; i++)
    {
      switch (i)
      {
        case 0: {
            curID = DXL_WHEEL_L;
            break;
          }
        case 1: {
            curID = DXL_WHEEL_R;
            break;
          }
        case 2: {
            curID = DXL_HEAD;
            break;
          }
        case 3: {
            curID = DXL_TAIL;
            break;
          }
      }

      dxl_wb.itemRead(curID, "Present_Velocity", &valuesSend[i][0]);
      valuesSend[i][0] = overflow65535(valuesSend[i][0]);
      dxl_wb.itemRead(curID, "Present_Position", &valuesSend[i][1]);
      valuesSend[i][1] = overflow65535(valuesSend[i][1]);
      dxl_wb.itemRead(curID, "Present_Current", &valuesSend[i][2]);
      valuesSend[i][2] = overflow65535(valuesSend[i][2]);

      switch (curID)
      {
        case DXL_WHEEL_L: {
            valuesSend[i][0] =   -motor_L_direction_fix[ID - 1] * valuesSend[i][0];
            valuesSend[i][1] =   -motor_L_direction_fix[ID - 1] * valuesSend[i][1];
            valuesSend[i][2] =   -motor_L_direction_fix[ID - 1] * valuesSend[i][2];
            break;
          }
        case DXL_WHEEL_R: {
            valuesSend[i][0] =   motor_L_direction_fix[ID - 1] * valuesSend[i][0];
            valuesSend[i][1] =   motor_R_direction_fix[ID - 1] * valuesSend[i][1];
            valuesSend[i][2] =   motor_R_direction_fix[ID - 1] * valuesSend[i][2];
            break;
          }
      }

      valuesSendLine[i] = String(valuesSend[i][0]) + "|" + String(valuesSend[i][1]) + "|" + String(valuesSend[i][2]);
    }

    String line = "0," + String(ID) + "," + valuesSendLine[0] + "|" + valuesSendLine[1] + "|" + valuesSendLine[2] + "|" + valuesSendLine[3] + "/";

    Serial.println(line);

    if (use_bt_2[ID - 1])
    {
      Serial2.println(line);
    }

    if (use_bt_4[ID - 1])
    {
      Serial4.println(line);
    }

  }

}

void valueSendOpen()
{
  Serial.println("Value send true");
  isValueSend = true;
  previousMillis = millis();
}

void valueSendClose()
{
  Serial.println("Value send false");
  isValueSend = false;
}

int32_t overflow65535(int32_t num)
{
  if (num >= 32768)
  {
    num = num - 65535;
  }

  return num;
}

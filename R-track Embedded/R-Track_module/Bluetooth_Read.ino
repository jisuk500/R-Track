

//-----------------------모든 블루투스 및 시리얼 통신 받아들이고 커멘드 분리----------------------------


void Bluetooth_Read(const int& serial_num)
{
  switch (serial_num)
  {
    case 0: {
        if (Serial.available())
        {
          for (int i = 0; i < 5; i++)
          {
            command_0[i] = "";
          }

          char temp;
          int i = 0;
          bool isNormal = true;
          while (i < 5)
          {
            if (Serial.available())
            {
              temp = Serial.read();
            }
            else continue;

            if (temp == ',') i = i + 1;
            else if (temp == 10) {
              isNormal = false;
              break;
            }
            else if (temp == 13) {
              isNormal = false;
              break;
            }
            else if (temp == '/') {
              isNormal = true;
              break;
            }
            else if (command_0[0] == "OK") {
              isNormal = false;
              break;
            }
            else
            {
              command_0[i] = command_0[i] + temp;
            }
          }

          if (isNormal) {
            //Serial.println("Serial0 : " + command_0[0] + "/" + command_0[1] + "/" + command_0[2] + "/" + command_0[3] + "/" + command_0[4]);

            Check_id_from_0();
          }
        }

        //Serial.flush();
        break;
      }
    case 2: {
        if (Serial2.available())
        {
          //Serial.println("Serial2 Recieving");
          for (int i = 0; i < 5; i++)
          {
            command_2[i] = "";
          }

          char temp;
          int i = 0;
          bool isNormal = true;
          while (i < 5)
          {
            if (Serial2.available())
            {
              temp = Serial2.read();
            }
            else continue;

            if (temp == ',') i = i + 1;
            else if (temp == 10) {
              isNormal = false;
              break;
            }
            else if (temp == 13) {
              isNormal = false;
              break;
            }
            else if (temp == '/') {
              isNormal = true;
              break;
            }
            else if (command_2[0] == "OK") {
              isNormal = false;
              break;
            }
            else
            {
              command_2[i] = command_2[i] + temp;
            }
          }
          if (isNormal) {
            //Serial.println("Serial2 : " + command_2[0] + "/" + command_2[1] + "/" + command_2[2] + "/" + command_2[3] + "/" + command_2[4]);
            Check_id_from_2();
          }
        }

        //Serial2.flush();
        break;
      }
    case 4: {
        if (Serial4.available())
        {
          //Serial.println("Serial4 Recieving");
          for (int i = 0; i < 5; i++)
          {
            command_4[i] = "";
          }

          char temp;
          int i = 0;
          bool isNormal = true;
          while (i < 5)
          {
            if (Serial4.available())
            {
              temp = Serial4.read();
            }
            else continue;

            if (temp == ',') i = i + 1;
            else if (temp == 10) {
              isNormal = false;
              break;
            }
            else if (temp == 13) {
              isNormal = false;
              break;
            }
            else if (temp == '/') {
              isNormal = true;
              break;
            }
            else if (command_4[0] == "OK") {
              isNormal = false;
              break;
            }
            else
            {
              command_4[i] = command_4[i] + temp;
            }
          }
          if (isNormal) {
            //Serial.println("Serial4 : " + command_4[0] + "/" + command_4[1] + "/" + command_4[2] + "/" + command_4[3] + "/" + command_4[4]);
            Check_id_from_4();
          }
        }

        //Serial4.flush();
        break;
      }
  }

}

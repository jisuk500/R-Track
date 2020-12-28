
//디버그 모드 바꾸는 함수
void change_debug_mode(const int& mode)
{
  switch (mode)
  {
    case 1: {
        if (current_mode == 1)
        {

        }
        else
        {
          Serial.println("Mode Changed to 1");
          current_mode = 1;
        }
        digitalWrite(led_pin_user[0], LOW);
        digitalWrite(led_pin_user[1], HIGH);
        break;
      }
    case 2: {
        if (current_mode == 2)
        {

        }
        else
        {
          Serial.println("Mode Changed to 2");
          current_mode = 2;
        }
        digitalWrite(led_pin_user[0], HIGH);
        digitalWrite(led_pin_user[1], LOW);
        break;
      }

    case 3: {
        if (current_mode == 3)
        {

        }
        else
        {
          Serial.println("Mode Changed to 3");
          current_mode = 3;
        }
        digitalWrite(led_pin_user[0], LOW);
        digitalWrite(led_pin_user[1], LOW);
        break;
      }
    default : {
        Serial.println("not a available mode number!");
        break;
      }
  }
}

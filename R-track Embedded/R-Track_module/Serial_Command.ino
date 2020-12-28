

//-------------------------------모든채널 공용 커맨드------------------------------------------

void Common_commands(String command[])
{
  if (command[1] == "MOVE")
  {
    if (command[2] == "A")
    {
      dxl_wb.goalVelocity(DXL_WHEEL_R, (int32_t)(speed_modes[ID - 1][current_mode - 1]*motor_R_direction_fix[ID - 1]));
      dxl_wb.goalVelocity(DXL_WHEEL_L, (int32_t)(-speed_modes[ID - 1][current_mode - 1]*motor_L_direction_fix[ID - 1]));
    }
    else if (command[2] == "D")
    {
      dxl_wb.goalVelocity(DXL_WHEEL_R, (int32_t)(0));
      dxl_wb.goalVelocity(DXL_WHEEL_L, (int32_t)(-0));
    }
    else
    {
      int goal_speed = command[2].toInt();
      goal_speed = constrain(goal_speed, -200, 200);
      dxl_wb.goalVelocity(DXL_WHEEL_R, (int32_t)(goal_speed * motor_R_direction_fix[ID - 1]));
      dxl_wb.goalVelocity(DXL_WHEEL_L, (int32_t)(-goal_speed * motor_L_direction_fix[ID - 1]));
    }
  }
  else if (command[1] == "TURN")
  {
    if (command[2] == "L")
    {
      dxl_wb.goalVelocity(DXL_WHEEL_R, (int32_t)(turn_speed * motor_R_direction_fix[ID - 1]));
      dxl_wb.goalVelocity(DXL_WHEEL_L, (int32_t)(-(-turn_speed) * motor_L_direction_fix[ID - 1]));
    }
    else if (command[2] == "R")
    {
      dxl_wb.goalVelocity(DXL_WHEEL_R, (int32_t)(-turn_speed * motor_R_direction_fix[ID - 1]));
      dxl_wb.goalVelocity(DXL_WHEEL_L, (int32_t)(-turn_speed * motor_L_direction_fix[ID - 1]));
    }
  }
  else if (command[1] == "HEAD-POS")
  {
    if (command[2] == "A")
    {
      dxl_wb.itemWrite(DXL_HEAD, "Goal_Position", (int32_t)(head_current[ID - 1]));
    }
    else if (command[2] == "D")
    {
      dxl_wb.itemWrite(DXL_HEAD, "Goal_Position", (int32_t)(0));
    }
    else
    {
      int current = command[2].toInt();
      current = constrain(current, 0, 4095);
      dxl_wb.itemWrite(DXL_HEAD, "Goal_Position", (int32_t)(current));
    }
  }
  else if (command[1] == "HEAD-AMP")
  {
    if (command[2] == "A")
    {
      dxl_wb.itemWrite(DXL_HEAD, "Goal_Current", (int32_t)(head_current[ID - 1]));
    }
    else if (command[2] == "D")
    {
      dxl_wb.itemWrite(DXL_HEAD, "Goal_Current", (int32_t)(0));
    }
    else
    {
      int current = command[2].toInt();
      current = constrain(current, 0, 1193);
      dxl_wb.itemWrite(DXL_HEAD, "Goal_Current", (int32_t)(current));
    }
  }
  else if (command[1] == "TAIL-POS")
  {
    if (command[2] == "A")
    {
      dxl_wb.itemWrite(DXL_TAIL, "Goal_Position", (int32_t)(tail_current[ID - 1]));
    }
    else if (command[2] == "D")
    {
      dxl_wb.itemWrite(DXL_TAIL, "Goal_Position", (int32_t)(0));
    }
    else
    {
      int current = command[2].toInt();
      current = constrain(current, 0, 4095);
      dxl_wb.itemWrite(DXL_TAIL, "Goal_Position", (int32_t)(current));
    }
  }
  else if (command[1] == "TAIL-AMP")
  {
    if (command[2] == "A")
    {
      dxl_wb.itemWrite(DXL_TAIL, "Goal_Current", (int32_t)(tail_current[ID - 1]));
    }
    else if (command[2] == "D")
    {
      dxl_wb.itemWrite(DXL_TAIL, "Goal_Current", (int32_t)(0));
    }
    else
    {
      int current = command[2].toInt();
      current = constrain(current, 0, 1193);
      dxl_wb.itemWrite(DXL_TAIL, "Goal_Current", (int32_t)(current));
    }
  }
  else if (command[1] == "LED")
  {
    if (command[2] == "1")
    {
      digitalWrite(led_pin_user[0], LOW);
    }
    else if (command[2] == "0")
    {
      digitalWrite(led_pin_user[0], HIGH);
    }
    else if (command[2] == "TEST")
    {

      String line = "0," + String(ID) + " is tested!/";
      Serial.println(line);
      if (use_bt_2[ID - 1] == true) Serial2.println(line);
      if (use_bt_4[ID - 1] == true) Serial4.println(line);


      if ((current_mode == 1) || (current_mode == 3))
      {
        digitalWrite(led_pin_user[0], HIGH);
        delay(2000);
        digitalWrite(led_pin_user[0], LOW);
      }
      else
      {
        digitalWrite(led_pin_user[0], LOW);
        delay(2000);
        digitalWrite(led_pin_user[0], HIGH);
      }
    }
  }
  else if (command[1] == "VALUE")
  {
    if (command[2] == "GIVE")
    {
      valueSendOpen();
    }
    else if (command[2] == "STOP")
    {
      valueSendClose();
    }
  }
  else if (command[1] == "TURNSPEED")
  {
    int current = command[2].toInt();
    current = constrain(current, 0, 200);
    turn_speed = current;
  }
  else if (command[1] == "HEAD-TAIL")
  {
    if (command[2] == "A")
    {
      dxl_wb.itemWrite(DXL_HEAD, "Goal_Position", (int32_t)(head_current[ID - 1]));
      dxl_wb.itemWrite(DXL_TAIL, "Goal_Current", (int32_t)(tail_current[ID - 1]));
    }
    else if (command[2] == "D")
    {
      dxl_wb.itemWrite(DXL_HEAD, "Goal_Position", (int32_t)(0));
      dxl_wb.itemWrite(DXL_TAIL, "Goal_Current", (int32_t)(0));
    }
    else
    {
      int current = command[2].toInt();
      current = constrain(current, -100, 100);
      dxl_wb.itemWrite(DXL_HEAD, "Goal_Position", (int32_t)(current));
      dxl_wb.itemWrite(DXL_TAIL, "Goal_Current", (int32_t)(current));
    }
  }
  else if (command[1] == "MODE")
  {
    if (command[2] == "TO")
    {
      int mode = command[3].toInt();
      change_debug_mode(mode);
    }
    else
    {
      int mode = command[2].toInt();
      mode = constrain(mode, 1, TOTAL_MODE_NUMBER);
      if (command[3] == "TO")
      {
        int want_speed = command[4].toInt();
        want_speed = constrain(want_speed, -200, 200);

        speed_modes[ID - 1][mode - 1] = want_speed;
        Serial.println("Change the mode[" + String(mode) + "] speed setting as : " + String(want_speed));
      }
    }
  }
}

//----------------------------------시리얼 0번 통신-------------------------------------

void Check_id_from_0()
{
  if (command_0[0] == String(ID)) //this
  {
    Common_commands(command_0);
    Serial_0_commands();
  }
  else if (command_0[0] == "255") //broadcast
  {
    if (use_bt_2[ID-1]) {
      String temp = command_0[0] + "," + command_0[1] + "," + command_0[2] + "," + command_0[3] + "," + command_0[4] + "/";
      Serial2.println(temp);
    }

    if (use_bt_4[ID-1]) {
      String temp = command_4[0] + "," + command_4[1] + "," + command_4[2] + "," + command_4[3] + "," + command_4[4] + "/";
      Serial4.println(temp);
    }

    Common_commands(command_0);
    Serial_0_commands();
  }
  else if ((command_0[0].toInt() >= 0) && (command_0[0].toInt() <= total_robot_num)) //else
  {
    if (use_bt_2[ID-1])
    {
      String temp = command_0[0] + "," + command_0[1] + "," + command_0[2] + "," + command_0[3] + "," + command_0[4] + "/";
      Serial2.println(temp);
    }

    if (use_bt_4[ID-1]) {
      String temp = command_4[0] + "," + command_4[1] + "," + command_4[2] + "," + command_4[3] + "," + command_4[4] + "/";
      Serial4.println(temp);
    }
  }
}


void Serial_0_commands()
{

}



//----------------------------------시리얼 2번 통신-------------------------------------

void Check_id_from_2()
{
  if (command_2[0] == String(ID)) //this
  {
    Common_commands(command_2);
    Serial_2_commands();
  }
  else if (command_2[0] == "255") //broadcast
  {
    if (use_bt_4[ID-1]) {
      String temp = command_2[0] + "," + command_2[1] + "," + command_2[2] + "," + command_2[3] + "," + command_2[4] + "/";
      Serial4.println(temp);
    }
    Common_commands(command_2);
    Serial_2_commands();
  }
  else if ((command_2[0].toInt() >= 0) && (command_2[0].toInt() <= total_robot_num)) //else
  {
    if (use_bt_4[ID-1])
    {
      String temp = command_2[0] + "," + command_2[1] + "," + command_2[2] + "," + command_2[3] + "," + command_2[4] + "/";
      Serial4.println(temp);
    }
  }
}


void Serial_2_commands()
{

}







//----------------------------------시리얼 4번 통신-------------------------------------

void Check_id_from_4()
{
  if (command_4[0] == String(ID)) //this
  {
    Common_commands(command_4);
    Serial_4_commands();
  }
  else if (command_4[0] == "255") //broadcast
  {
    if (use_bt_2[ID-1]) {
      String temp = command_4[0] + "," + command_4[1] + "," + command_4[2] + "," + command_4[3] + "," + command_4[4] + "/";
      Serial2.println(temp);
    }

    Common_commands(command_4);
    Serial_4_commands();
  }
  else if ((command_4[0].toInt() >= 0) && (command_4[0].toInt() <= total_robot_num)) //else
  {
    if (use_bt_2[ID-1])
    {
      String temp = command_4[0] + "," + command_4[1] + "," + command_4[2] + "," + command_4[3] + "," + command_4[4] + "/";
      Serial2.println(temp);
    }
  }
}


void Serial_4_commands()
{

}

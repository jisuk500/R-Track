// -----------------------------------------------이 로봇의 아이디
#define ID 1
#define total_robot_num 3 //연결된 로봇의 총 개수 (0부터 시작 아님)

//---------------------------------------------실험용 모드 관련
#define TOTAL_MODE_NUMBER 3
int current_mode = 1;

int speed_modes[total_robot_num][TOTAL_MODE_NUMBER] = {
  {5, 10, -5},               //ID 1
  {5, 10, -5},               //ID 2
  {70, 70, 70}                //ID 3
};

//-------------------------------------------통신 관련
String command_0[5] = {"", "", "", "", ""};
String command_2[5] = {"", "", "", "", ""};
String command_4[5] = {"", "", "", "", ""};

bool use_bt_2[total_robot_num] = {true, true, true};
bool use_bt_4[total_robot_num] = {true, true, false};

//--------------------------------------------모터 제어 관련
int head_current[total_robot_num] = {0, 30, 30};
int tail_current[total_robot_num] = {30, 30, 60};

int turn_speed = 10;

int motor_L_direction_fix[total_robot_num] = { 1, 1, 1};
int motor_R_direction_fix[total_robot_num] = { 1, 1, -1};



//----------------------------------------유저 엘이디 핀번호
#define BDPIN_LED_USER_1        22
#define BDPIN_LED_USER_2        23
#define BDPIN_LED_USER_3        24
#define BDPIN_LED_USER_4        25
int led_pin_user[4] = { BDPIN_LED_USER_1, BDPIN_LED_USER_2, BDPIN_LED_USER_3, BDPIN_LED_USER_4 };




//------------------------------------------다이나믹셀 관련
#include <DynamixelWorkbench.h>

#if defined(__OPENCM904__)
#define DEVICE_NAME "3" //Dynamixel on Serial3(USART3)  <-OpenCM 485EXP
#elif defined(__OPENCR__)
#define DEVICE_NAME ""
#endif

#define BAUDRATE  57600
#define DXL_WHEEL_R 3
#define DXL_WHEEL_L 1
#define DXL_TAIL 2
#define DXL_HEAD 4

DynamixelWorkbench dxl_wb;



//-----------------------------------------------각 모터들의 수치 읽어오는것 관련 변수

bool isValueSend = false;
unsigned long currentMillis = 0;
unsigned long previousMillis = 0;
unsigned long millisDiff = 0;

int32_t valuesSend[4][3] = {{0, 0, 0}, {0, 0, 0}, { 0, 0, 0}, { 0, 0, 0}};
String valuesSendLine[] = {"", "", "", ""};
int32_t get_data = 0;

int32_t valueSendDelay = 250;



void setup() {
  Serial.begin(57600);


  if (use_bt_2[ID - 1]) Serial2.begin(57600); //slot2 bt
  if (use_bt_4[ID - 1]) Serial4.begin(57600); //slot4 bt
  delay(10);

  Serial.println("BT Connection Check start");
  Serial.println("press user butt 1 to skip");
  Check_bt_connection();
  Serial.println("BT Connections Ready!");
  digitalWrite(led_pin_user[3], LOW);

  dxl_wb.begin(DEVICE_NAME, BAUDRATE);//모터들 부팅 시작 ( 1234번 모터가 다 연결되어있지 않으면 여기서 안넘어가니 주의)
  dxl_wb.ping(DXL_WHEEL_R);
  dxl_wb.ping(DXL_WHEEL_L);
  dxl_wb.ping(DXL_TAIL);
  dxl_wb.ping(DXL_HEAD);
  Serial.println("Motor Boot Complete");

  dxl_wb.wheelMode(DXL_WHEEL_R);
  dxl_wb.wheelMode(DXL_WHEEL_L);

  dxl_wb.currentBasedPositionMode(DXL_TAIL);
  dxl_wb.currentBasedPositionMode(DXL_HEAD);
  delay(100);
  dxl_wb.torque(DXL_TAIL, true);
  dxl_wb.torque(DXL_HEAD, true);
  Serial.println("Motor Drive Mode Setting Complete");

  change_debug_mode(current_mode);
  Serial.println("load the robot parameter default as mode number : " + String(current_mode));


  Serial.println("Entering the main Loop..");
  digitalWrite(led_pin_user[2], LOW);
}

void loop() {
  Bluetooth_Read(0);
  if (use_bt_2[ID - 1] == true) Bluetooth_Read(2);
  if (use_bt_4[ID - 1] == true) Bluetooth_Read(4);

  if (isValueSend == true)checkValueSend();
}

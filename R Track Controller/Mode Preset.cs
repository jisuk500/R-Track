using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace R_Track_Controller
{
    
    [Serializable]
    /// <summary>
    /// 모드 프리셋을 담당
    /// </summary>
    public class Mode_Presets
    {
        /// <summary>
        /// 모드들 정보가 저장된 리스트
        /// </summary>
        public List<mode> modes { get; set; }

        [Serializable]
        /// <summary>
        /// 모드 하나의 클래스
        /// </summary>
        public class mode
        {
            /// <summary>
            /// 이 모드의 이름
            /// </summary>
            public string modeName { get; set; }

            /// <summary>
            /// 모드 하나에 들어있는 로봇 3개짜리 배열
            /// </summary>
            public List<Robot> Robots { get; set; }

            [Serializable]
            /// <summary>
            /// 로봇 하나 안에 들어있는 프리셋들 클래스
            /// </summary>
            public class Robot
            {
                /// <summary>
                /// 로봇 하나 안에 들어있는 속도, 헤드, 테일 순서의 노드내용
                /// </summary>
                public List<node> nodes { get; set; }

                [Serializable]
                /// <summary>
                /// 하나의 노드에 대한 클래스
                /// </summary>
                public class node
                {
                    /// <summary>
                    /// 노드 하나의 기존 값
                    /// </summary>
                    public int value { get; set; }
                    /// <summary>
                    /// 노드 하나의 새로운 값
                    /// </summary>
                    public int value_new { get; set; }
                    /// <summary>
                    /// 기존값을 사용 했었는지
                    /// </summary>
                    public bool value_available { get; set; }
                    /// <summary>
                    /// 새로운 값은 사용 할 건지
                    /// </summary>
                    public bool value_available_new { get; set; }

                    /// <summary>
                    /// node 클래스의 생성자. 전부 0,false로 되어있음
                    /// </summary>
                    public node()
                    {
                        value = 0;
                        value_new = 0;
                        value_available = true;
                        value_available_new = true;
                    }
                }

                /// <summary>
                /// 로봇 클래스의 생성자. 자동으로 3개의 node(속도,헤드,테일)를 만들어서 놓는다
                /// </summary>
                public Robot()
                {
                    nodes = new List<node>();
                    for (int i=0;i<5;i++)
                    {
                        nodes.Add(new node());
                    }
                }
            }

            /// <summary>
            /// mode 클래스의 생성자
            /// </summary>
            public mode()
            {
                modeName = "mode_name";
                Robots = new List<Robot>();

                for(int i=0;i<3;i++)
                {
                    Robots.Add(new Robot());
                }
            }
        }

        /// <summary>
        /// 모드 프리셋 클래스 초기화
        /// </summary>
        public Mode_Presets()
        {
            modes = new List<mode>();
            for(int i=0;i<5;i++)
            {
                modes.Add(new mode());
            }
        }

    }

    /// <summary>
    /// 모드 프리셋 입출력 담당 정적 클래스
    /// </summary>
    public static class mode_preset_io
    {
        /// <summary>
        /// 안에 담긴 현재의 모드 프리셋들
        /// </summary>
        public static Mode_Presets Presets { get; set; }

        public static bool savePresets()
        {
            Stream ws = new FileStream("mode_presets.dat", FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();

            try
            {
                serializer.Serialize(ws, Presets);
            }
            catch(Exception e)
            {
                ws.Close();
                return false;
            }
            ws.Close();
            return true;
        }

        public static bool readPresets()
        {
            try
            {
                Stream rs = new FileStream("mode_presets.dat", FileMode.Open);
                rs.Close();
            }
            catch
            {
                savePresets();
                return false;
            }

            try
            {
                Stream rs = new FileStream("mode_presets.dat", FileMode.Open);
                BinaryFormatter deserializer = new BinaryFormatter();
                Presets = (Mode_Presets)deserializer.Deserialize(rs);
                rs.Close();
            }
            catch(Exception e)
            {
                
                return false;
            }
            return true;
        }

        public static void initialize()
        {
            Presets = new Mode_Presets();
        }

        public static void updateNewtoNow()
        {
            foreach(var mode in Presets.modes)
            {
                foreach(var robot in mode.Robots)
                {
                    foreach(var node in robot.nodes)
                    {
                        node.value = node.value_new;
                        node.value_available = node.value_available_new;
                    }
                }
            }
        }

    }
    
}

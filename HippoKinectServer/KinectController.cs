using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using Microsoft.Kinect;

namespace HippoKinectServer {
    class KinectController {
        public static Queue action_list;

        private static KinectSensor sensor;

        public static void Init() {
            action_list = new Queue();
            lock (action_list) {
                action_list.Clear();
            }

            foreach (var potentialSensor in KinectSensor.KinectSensors) {
                if (potentialSensor.Status == KinectStatus.Connected) {
                    sensor = potentialSensor;
                    break;
                }
            }

            if (sensor == null) {
                GlobalHelper.LOG("ERROR", "Can't find an available Kinect sensor.");
            } else {
                sensor.SkeletonStream.Enable();
                sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(SensorSkeletonFrameReady);
                try {
                    sensor.Start();
                } catch (IOException) {
                    sensor = null;
                    GlobalHelper.LOG("ERROR", "Kinect Sensor Start Error.");
                }
            }
        }

        public static int Status() {
            if (sensor == null) return 1;
            if (sensor.Status != KinectStatus.Connected) return 2;
            return 0;
        }

        public void Watch() {

        }

        private static void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e) {
            Skeleton[] skeletons = new Skeleton[0];
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame()) {
                if (skeletonFrame != null) {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }
            
        }
    }
}

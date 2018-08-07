using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Formats.Alembic.Exporter;
using UnityEngine.Formats.Alembic.Sdk;
using UnityEngine.Formats.Alembic.Timeline;
using UnityEngine.Formats.Alembic.Util;
using UnityEngine.TestTools;

namespace UnityEditor.Formats.Alembic.Exporter.UnitTests {
    public class AlembicAPITest : AlembicTestBase {

        // Loads a given scene
            IEnumerator SceneLoader (string sceneToLoad) {
            SceneManagement.EditorSceneManager.LoadScene (sceneToLoad, UnityEngine.SceneManagement.LoadSceneMode.Single);
            yield return null;
        }

        // Records a frame then stops the recorder
        IEnumerator Record (AlembicRecorder recorderTarget) {
            var result = recorderTarget.BeginRecording ();
            Assert.That (result, Is.True);
            recorderTarget.ProcessRecording ();
            yield return null;
            recorderTarget.EndRecording ();
        }

        // Test Capture Components
        [UnityTest]
        public IEnumerator TestCaptureComponents () {
            SceneLoader ("TestAPI");
            using (AlembicRecorder recorder = new AlembicRecorder ()) {
                recorder.settings.OutputPath = GetRandomAbcFilePath();
                recorder.settings.AssumeNonSkinnedMeshesAreConstant = false;
                recorder.settings.CaptureMeshRenderer = true;
                recorder.settings.CaptureCamera = true;
                recorder.settings.MeshSubmeshes = true;
                Record (recorder);
            }
            yield return null;
        }

        // Test AeConfig Settings
        [UnityTest]
        public IEnumerator TestAeConfig () {
            SceneLoader ("TestAPI");
            using (AlembicRecorder recorder = new AlembicRecorder ()) {
                recorder.settings.OutputPath = GetRandomAbcFilePath();
                var conf = recorder.settings.Conf;
                conf.ArchiveType = UnityEngine.Formats.Alembic.Sdk.aeArchiveType.Ogawa;
                conf.FrameRate = 17;
                conf.ScaleFactor = 1;
                conf.SwapHandedness = true;
                Record (recorder);
            }
            yield return null;
        }

        // Test Dispose current recording
        [UnityTest]
        public IEnumerator TestDispose () {
            SceneLoader ("TestAPI");
            AlembicRecorder recorder;
            using(recorder = new AlembicRecorder()){}
            Assert.That(recorder.settings, Is.Not.Null);
            recorder = new AlembicRecorder();
            recorder.Dispose();
            Assert.That(recorder.settings, Is.Not.Null);
            yield return null;
        }

        // Test the Bool class
        [UnityTest]
        public IEnumerator TestBool () {
            SceneLoader ("TestAPI");
            var alembicBool = Bool.ToBool(true);
            var regularBool = Bool.ToBool(alembicBool);
            Assert.AreEqual(regularBool,true);
            yield return null;
        }
    }
}
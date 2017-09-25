pipeline {
  agent none
  def UNITY_PATH      =    "/Applications/Unity/Unity.app/Contents/MacOS/Unity";
  def JENKINS_FILE_ROOT_PATH = pwd();
  stages {
    stage('error') {
      steps {
        parallel(
          "ABC": {
            echo 'hello world'
            echo "${UNITY_PATH}"
          },
          "DEF": {
            echo 'go'
            echo "${UNITY_PATH}"
          }
        )
      }
    }
    stage('End') {
      steps {
        echo 'end'
        echo "${UNITY_PATH}"
      }
    }
  }
}

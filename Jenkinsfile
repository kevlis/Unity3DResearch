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
            
          },
          "DEF": {
            echo 'go'
            
          }
        )
      }
    }
    stage('End') {
      steps {
        echo 'end'
      }
    }
  }
}

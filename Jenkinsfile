pipeline {
  agent none
  stages {
    stage('error') {
      steps {
        parallel(
          "Hello": {
            echo 'hello world'
            
          },
          "GO": {
            sh 'go'
            
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
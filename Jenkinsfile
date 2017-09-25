pipeline {
  define {
    def AAV = "AVCD";
  }
  agent none
  stages {
    stage('error') {
      steps {
            parallel(
              "ABC": {
                echo 'hello world'
                  script {
                    AAV = "ADV"  
                  }
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
        echo "${AAV}"
      }
    }
  }
}

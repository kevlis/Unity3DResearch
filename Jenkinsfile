pipeline {

  agent none
  stages {
    stage('error') {
      steps {
            parallel(
              "ABC": {
                echo 'hello world'
                local AAV = "AVCD"
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

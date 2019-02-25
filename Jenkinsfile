def image

pipeline {
    agent none
    environment
    {
      ECRURL = '989299900151.dkr.ecr.ap-southeast-1.amazonaws.com'
      ECRCRED = 'ecr:ap-southeast-1:aws-client-creds'
      REPOSITORY = 'sprintcrowd/backend'
      DEPLOYSERVER = 'ubuntu@18.136.179.161'
    }
    stages {
        stage("build") {
            agent { label 'LinuxSlave' }
            when { anyOf { branch 'test'; branch 'development' } }
            steps {
                script {
                    image = docker.build("${env.REPOSITORY}:${env.BRANCH_NAME}.${env.BUILD_ID}")
                }
            }
        }
        stage("push-image") {
            agent { label 'LinuxSlave' }
            when { anyOf { branch 'test'; branch 'development' } }
            steps {
                script {
                    docker.withRegistry("https://${env.ECRURL}", ECRCRED) {
                        image.push("${env.BRANCH_NAME}.${env.BUILD_ID}")
                        image.push("${env.BRANCH_NAME}.latest")
                    }
                   sh "docker rmi -f ${env.ECRURL}/${env.REPOSITORY}:${env.BRANCH_NAME}.${env.BUILD_ID} ${env.ECRURL}/${env.REPOSITORY}:${env.BRANCH_NAME}.latest ${env.REPOSITORY}:${env.BRANCH_NAME}.${env.BUILD_ID}"
                }
            }
        }
        stage("deploy") {
          agent { label 'scrowd-slave' }
          when {
                branch 'test'
          }
          steps {
            script {
                docker.withRegistry("https://${env.ECRURL}", ECRCRED) {
                  sh 'pwd'
                  sh 'cd ~/devops; ls -la; git pull'
                  sh 'cd ~/devops/sprintcrowd-backend/prod; chmod 744 ./deploy.sh; ./deploy.sh'
                }
            }
          }
        }
    }
}
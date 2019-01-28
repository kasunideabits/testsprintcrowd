def image

pipeline {
    agent { label 'LinuxSlave' }
    stages {
        stage("build") {
            steps {
                script {
                    image = docker.build("sprintcrowd-backend:${env.BRANCH_NAME}.${env.BUILD_ID}")
                }
            }
        }
        // stage("push-image") {
        //     steps {
        //         script {
        //             docker.withRegistry('https://gcr.io/', 'gcr:nextory-adbox') {
        //                 image.push("${env.BRANCH_NAME}.${env.BUILD_ID}")
        //                 image.push("${env.BRANCH_NAME}.latest")
        //             }
        //         }
        //     }
        // }
        // stage("deploy-dev") {
        //     when {
        //         branch 'development'
        //     }
        //     steps {
        //         sshagent(credentials: ['jenkins-ssh']) {
        //             sh 'ssh -o StrictHostKeyChecking=no jenkins@nextory-adbox.dev.z-acceleration.net "cd devops; git pull"'
        //             sh 'ssh jenkins@nextory-adbox.dev.z-acceleration.net "cd devops/dev; ./deploy.sh"'
        //         }
        //     }
        // }

        // stage("deploy") {
        //     when {
        //         branch 'master'
        //     }
        //     steps {
        //         sshagent(credentials: ['jenkins-ssh']) {
        //             sh 'ssh -o StrictHostKeyChecking=no jenkins@nextory-adbox.dev.z-acceleration.net "cd devops; git pull"'
        //             sh 'ssh jenkins@nextory-adbox.dev.z-acceleration.net "cd devops/prod; ./deploy.sh"'
        //         }
        //     }
        // }

    }
}

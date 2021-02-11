Integration against Active Directory, consisting of two applications

## backend
This is a Java 11, Spring Boot, application, that can run off-premise. It exposes REST endpoints for looking up who has a certain role in
Active Directory, and communicates with an on-premise Windows Service through secure websockets.

## winservice
This is the C# windows service, which is installed on-premise and granted access to read from Active Directory. It communicates with
the above Java application through secure websockets, allowing the Java application to query for group memberships


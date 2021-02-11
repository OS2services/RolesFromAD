package dk.digitalidentity.service.model;

import java.util.Objects;

import dk.digitalidentity.service.HMacUtil;
import lombok.Getter;
import lombok.Setter;
import lombok.extern.slf4j.Slf4j;

@Slf4j
@Getter
@Setter
public class Response {
	private String transactionUuid;   // echo transactionUuid from request
	private String command;           // echo command from request
	private String status;            // true on success, false otherwise
	private String clientVersion;     // version of client
	private String signature;         // keyed hmac on above
	private Object payload;

	public boolean verify(String key) {
		switch (command) {
			case "AUTHENTICATE":
			case "GET_USERS_WITH_ROLE":
				try {
					return Objects.equals(this.signature, HMacUtil.hmac(transactionUuid + "." + command + "." + status, key));
				}
				catch (Exception ex) {
					log.error("Failed to verify signature", ex);

					return false;
				}
			default:
				log.error("Unknown command: " + command);
				break;
		}
		
		return false;
	}
}

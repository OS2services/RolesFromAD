package dk.digitalidentity.config;

import java.util.ArrayList;
import java.util.List;

import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.stereotype.Component;

import dk.digitalidentity.config.model.RoleIdentifierMapping;
import dk.digitalidentity.controller.api.dto.UserDTO;
import lombok.Getter;
import lombok.Setter;

@Component
@Getter
@Setter
@ConfigurationProperties(prefix = "configuration")
public class AppConfiguration {
	private boolean enabled;
	private String apiKey;
	private String webSocketKey;

	private List<UserDTO> testData;
	private List<RoleIdentifierMapping> mapping = new ArrayList<>();
}

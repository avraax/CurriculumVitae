# CurriculumVitae API - Postman Testing Guide

This guide provides comprehensive instructions for testing the CurriculumVitae GraphQL API using Postman, including examples for all available operations.

## Table of Contents
- [Setup](#setup)
- [GraphQL Basics in Postman](#graphql-basics-in-postman)
- [Authentication](#authentication)
- [Available Queries](#available-queries)
- [Query Examples](#query-examples)
- [Error Handling](#error-handling)
- [Best Practices](#best-practices)
- [Troubleshooting](#troubleshooting)

## Setup

### Prerequisites
- Postman installed on your machine
- CurriculumVitae API running locally (default: `http://localhost:5000`)

### Postman Collection Setup
1. Create a new collection in Postman called "CurriculumVitae API"
2. Set up environment variables:
   - `baseUrl`: `http://localhost:5000` (or your deployed URL)
   - `graphqlEndpoint`: `{{baseUrl}}/graphql`

### Basic Request Configuration
All requests should be configured as follows:
- **Method**: `POST`
- **URL**: `{{graphqlEndpoint}}`
- **Headers**:
  ```
  Content-Type: application/json
  ```

## GraphQL Basics in Postman

### Request Structure
All GraphQL requests use the same endpoint with different query bodies:

```json
{
  "query": "your GraphQL query here",
  "variables": {
    "variableName": "value"
  },
  "operationName": "OptionalOperationName"
}
```

### Response Format
GraphQL responses follow this structure:
```json
{
  "data": {
    // Your requested data
  },
  "errors": [
    // Array of errors if any occurred
  ]
}
```

## Authentication

Currently, the API doesn't require authentication. All endpoints are publicly accessible.

## Available Queries

The CurriculumVitae API supports the following queries:

| Query | Description | Parameters |
|-------|-------------|------------|
| `person` | Get complete CV profile | None |
| `workExperiences` | Get all work experiences | None |
| `workExperience` | Get specific work experience | `id: Int!` |
| `educations` | Get all education records | None |
| `education` | Get specific education record | `id: Int!` |
| `skills` | Get all skills | None |
| `skill` | Get specific skill | `id: Int!` |
| `projects` | Get all projects | None |
| `project` | Get specific project | `id: Int!` |
| `companies` | Get all companies | None |
| `company` | Get specific company | `id: Int!` |

## Query Examples

### 1. Get Person Profile (Complete CV)

**Request Name**: `Get Person Profile`

**Body**:
```json
{
  "query": "query GetPerson { person { id fullName email phone summary workExperiences { id company position startDate endDate description isCurrent } educations { id institution degree fieldOfStudy startDate endDate grade description isInProgress } skills { id name proficiencyLevel category yearsOfExperience description } projects { id name description technologies startDate endDate projectUrl sourceCodeUrl isFeatured isOngoing technologiesList } } }"
}
```

**Formatted Query** (for readability):
```graphql
query GetPerson {
  person {
    id
    fullName
    email
    phone
    summary
    workExperiences {
      id
      company
      position
      startDate
      endDate
      description
      isCurrent
    }
    educations {
      id
      institution
      degree
      fieldOfStudy
      startDate
      endDate
      grade
      description
      isInProgress
    }
    skills {
      id
      name
      proficiencyLevel
      category
      yearsOfExperience
      description
    }
    projects {
      id
      name
      description
      technologies
      startDate
      endDate
      projectUrl
      sourceCodeUrl
      isFeatured
      isOngoing
      technologiesList
    }
  }
}
```

### 2. Get All Work Experiences

**Request Name**: `Get All Work Experiences`

**Body**:
```json
{
  "query": "query GetWorkExperiences { workExperiences { id company position startDate endDate description isCurrent } }"
}
```

### 3. Get Specific Work Experience

**Request Name**: `Get Work Experience by ID`

**Body**:
```json
{
  "query": "query GetWorkExperience($id: Int!) { workExperience(id: $id) { id company position startDate endDate description isCurrent } }",
  "variables": {
    "id": 1
  }
}
```

### 4. Get All Education Records

**Request Name**: `Get All Education Records`

**Body**:
```json
{
  "query": "query GetEducations { educations { id institution degree fieldOfStudy startDate endDate grade description isInProgress } }"
}
```

### 5. Get Specific Education Record

**Request Name**: `Get Education by ID`

**Body**:
```json
{
  "query": "query GetEducation($id: Int!) { education(id: $id) { id institution degree fieldOfStudy startDate endDate grade description isInProgress } }",
  "variables": {
    "id": 1
  }
}
```

### 6. Get All Skills

**Request Name**: `Get All Skills`

**Body**:
```json
{
  "query": "query GetSkills { skills { id name proficiencyLevel category yearsOfExperience description } }"
}
```

### 7. Get Specific Skill

**Request Name**: `Get Skill by ID`

**Body**:
```json
{
  "query": "query GetSkill($id: Int!) { skill(id: $id) { id name proficiencyLevel category yearsOfExperience description } }",
  "variables": {
    "id": 1
  }
}
```

### 8. Get All Projects

**Request Name**: `Get All Projects`

**Body**:
```json
{
  "query": "query GetProjects { projects { id name description technologies startDate endDate projectUrl sourceCodeUrl isFeatured isOngoing technologiesList } }"
}
```

### 9. Get Specific Project

**Request Name**: `Get Project by ID`

**Body**:
```json
{
  "query": "query GetProject($id: Int!) { project(id: $id) { id name description technologies startDate endDate projectUrl sourceCodeUrl isFeatured isOngoing technologiesList } }",
  "variables": {
    "id": 1
  }
}
```

### 10. Get All Companies

**Request Name**: `Get All Companies`

**Body**:
```json
{
  "query": "query GetCompanies { companies { id name description industry location website employeeCount foundedDate notes } }"
}
```

### 11. Get Specific Company

**Request Name**: `Get Company by ID`

**Body**:
```json
{
  "query": "query GetCompany($id: Int!) { company(id: $id) { id name description industry location website employeeCount foundedDate notes } }",
  "variables": {
    "id": 1
  }
}
```

### 12. Get Skills by Category

**Request Name**: `Get Technical Skills Only`

**Body**:
```json
{
  "query": "query GetSkills { skills { id name proficiencyLevel category yearsOfExperience description } }"
}
```

*Note: Filter by category in your application logic or use the results as needed.*

### 13. Get Current Work Experiences

**Request Name**: `Get Current Work Experiences`

**Body**:
```json
{
  "query": "query GetWorkExperiences { workExperiences { id company position startDate endDate description isCurrent } }"
}
```

*Note: Use the `isCurrent` field to filter current positions.*

### 14. Get Featured Projects

**Request Name**: `Get Featured Projects`

**Body**:
```json
{
  "query": "query GetProjects { projects { id name description technologies startDate endDate projectUrl sourceCodeUrl isFeatured isOngoing technologiesList } }"
}
```

*Note: Use the `isFeatured` field to filter featured projects.*

## Advanced Query Examples

### 15. Selective Field Query

**Request Name**: `Get Person Basic Info`

**Body**:
```json
{
  "query": "query GetPersonBasic { person { fullName email phone } }"
}
```

### 16. Complex Nested Query

**Request Name**: `Get Person with Current Work and Featured Projects`

**Body**:
```json
{
  "query": "query GetPersonDetailed { person { fullName email summary workExperiences { company position isCurrent } projects { name isFeatured projectUrl } } }"
}
```

### 17. Multiple Queries in One Request

**Request Name**: `Get Summary Data`

**Body**:
```json
{
  "query": "query GetSummary { person { fullName email } workExperiences { company position isCurrent } skills { name proficiencyLevel category } }"
}
```

## Error Handling

### Common Error Scenarios

1. **Invalid Query Syntax**:
```json
{
  "errors": [
    {
      "message": "Syntax Error: Expected Name, found }",
      "locations": [{"line": 1, "column": 15}]
    }
  ]
}
```

2. **Invalid Field Name**:
```json
{
  "errors": [
    {
      "message": "Cannot query field \"invalidField\" on type \"Person\"."
    }
  ]
}
```

3. **Missing Required Variable**:
```json
{
  "errors": [
    {
      "message": "Variable \"$id\" of required type \"Int!\" was not provided."
    }
  ]
}
```

4. **Invalid Variable Type**:
```json
{
  "errors": [
    {
      "message": "Variable \"$id\" got invalid value \"abc\"; Expected type \"Int\"."
    }
  ]
}
```

### Testing Error Scenarios

Create these requests to test error handling:

**Invalid Query Syntax**:
```json
{
  "query": "query { person { invalidField } }"
}
```

**Missing Required Parameter**:
```json
{
  "query": "query { workExperience { id company } }"
}
```

## Best Practices

### 1. Request Organization
- Group related queries in folders (Person, Work Experience, Education, Skills, Projects, Companies)
- Use descriptive names for your requests
- Add descriptions to document the purpose of each request

### 2. Variable Usage
- Always use variables for dynamic values
- Test with both valid and invalid variable values
- Document expected variable types in request descriptions

### 3. Field Selection
- Only request fields you need to minimize response size
- Use nested queries appropriately
- Test with minimal and maximal field selections

### 4. Environment Management
- Use environment variables for URLs and common values
- Set up different environments (Development, Staging, Production)
- Keep sensitive data in environment variables

### 5. Testing Strategy
- Test all queries with valid data
- Test error scenarios
- Verify response data structure
- Check for null values handling

## Troubleshooting

### Common Issues and Solutions

1. **Connection Refused**:
   - Ensure the API server is running
   - Check if the port number is correct
   - Verify the base URL in environment variables

2. **Empty Response Data**:
   - Check if the database has been initialized
   - Verify the query syntax
   - Ensure proper field names are used

3. **Slow Response Times**:
   - Consider reducing the number of requested fields
   - Check if the database needs optimization
   - Monitor server performance

4. **Malformed JSON**:
   - Validate JSON syntax in request body
   - Ensure proper escaping of quotes in queries
   - Use Postman's JSON validator

### Debug Tips

1. **Enable Postman Console**:
   - View > Show Postman Console
   - Monitor request/response details

2. **Use Query Variables**:
   - Test with different ID values
   - Verify variable substitution

3. **Check Response Headers**:
   - Verify Content-Type is application/json
   - Check for any custom headers

4. **Test Incrementally**:
   - Start with simple queries
   - Gradually add complexity
   - Test one field at a time when debugging

## Sample Test Scenarios

### Scenario 1: Complete CV Data Validation
1. Get person profile with all nested data
2. Verify all required fields are present
3. Validate data types and formats
4. Check for consistent person IDs across related entities

### Scenario 2: Individual Entity Testing
1. Test each entity query independently
2. Verify specific ID lookups work correctly
3. Test with various ID values (valid, invalid, non-existent)
4. Validate computed fields (isCurrent, isOngoing, etc.)

### Scenario 3: Performance Testing
1. Measure response times for different query complexities
2. Test with large result sets
3. Monitor memory usage patterns
4. Validate caching behavior if implemented

### Scenario 4: Edge Case Testing
1. Test with empty database
2. Test with null/empty field values
3. Test with special characters in text fields
4. Validate date field formats and ranges

This comprehensive guide should help you thoroughly test all aspects of the CurriculumVitae API using Postman. Remember to adapt the examples based on your specific data and testing requirements. 
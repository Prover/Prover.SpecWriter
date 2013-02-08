require 'rake/clean'
require 'bundler/setup'
require 'albacore'
require 'semver'
require 'albacore/tasks/versionizer'
require 'albacore/ext/teamcity'

Albacore::Tasks::Versionizer.new :versioning



desc 'compile Prover.SpecWriter.sln'
build :build => :versioning do |b|
  b.sln = File.join 'src', 'Prover.SpecWriter-VS2012.sln'
  # b.logging = 'normal'
end

task :default => :build

directory 'build/pkg'
namespace :nuget do
	desc "create a nuget for Prover.SpecWriter"
  nugets_pack :pack => ['build/pkg', :versioning] do |c|
    c.files   = FileList['src/Prover.SpecWriter/Prover.SpecWriter.csproj']
		c.version = ENV['NUGET_VERSION']
		c.out     = 'build/pkg'
		c.exe     = 'src/.nuget/NuGet.exe'
  end

	desc "release a new version #{`semver`}"
	task :release => :"nuget:pack" do
		sh "src/.nuget/NuGet.exe push build\\pkg\\Prover.SpecWriter.#{ENV['NUGET_VERSION']}.nupkg -ApiKey #{ENV['NUGET_API_KEY']}"
		sh "git tag #{`semver`}"
	  puts 'Now do \'git push && git push --tags\''
	end
end
